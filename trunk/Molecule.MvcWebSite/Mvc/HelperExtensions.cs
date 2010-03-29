using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Molecule.MvcWebSite.Controllers;
using Microsoft.Web.Mvc.Internal;
using System.Linq.Expressions;
using System.IO;
using System.Reflection;
using System.Web.Routing;
using System.Text;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc.Html;
using Molecule.WebSite.Services;

namespace Molecule.Web.Mvc
{
    

    public static class HelperExtensions
    {
        const string themeKey = "theme";

        public static string Action<T>(this UrlHelper helper, Expression<Action<T>> action)
            where T : PublicPageControllerBase
        {
            var routeValues = ExpressionHelper.GetRouteValuesFromExpression(action);
            routeValues = new RouteValueDictionary(routeValues.ToDictionary(p => p.Key.ToLower(), p => p.Value));
            routeValues.Add("atome", PublicPageControllerBase.GetAtome<T>().Id);
            return helper.RouteUrl(routeValues);
        }

        public static string ActionLink<T>(this HtmlHelper helper, string linkText, Expression<Action<T>> action)
           where T : PublicPageControllerBase
        {
            var url = new UrlHelper(helper.ViewContext.RequestContext).Action(action);

            var tagBuilder = new TagBuilder("a"){
                InnerHtml = (!String.IsNullOrEmpty(linkText)) ? HttpUtility.HtmlEncode(linkText) : String.Empty
            };
            //tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("href", url);
            return tagBuilder.ToString(TagRenderMode.Normal);
        }

        public static TagBuilder ActionLink<T>(this HtmlHelper helper, Expression<Action<T>> action)
            where T : PublicPageControllerBase
        {

            var url = new UrlHelper(helper.ViewContext.RequestContext).Action(action);

            var tagBuilder = new TagBuilder(helper.ViewContext.HttpContext.Response, "a");
            tagBuilder.MergeAttribute("href", url);

            tagBuilder.Write(TagRenderMode.StartTag);
            return tagBuilder;
        }

        public static MvcForm BeginForm<T>(this HtmlHelper helper, Expression<Action<T>> action)
            where T : PublicPageControllerBase
        {
            return BeginForm<T>(helper, action, FormMethod.Post, null);
        }

        public static MvcForm BeginForm<T>(this HtmlHelper helper, Expression<Action<T>> action,
                FormMethod method, object htmlAttributes)
            where T : PublicPageControllerBase
        {
            var routeValues = ExpressionHelper.GetRouteValuesFromExpression(action);
            routeValues = new RouteValueDictionary(routeValues.ToDictionary(p => p.Key.ToLower(), p => p.Value));
            routeValues.Add("atome", PublicPageControllerBase.GetAtome<T>().Id);
            return helper.BeginForm((string)null, (string)null, (RouteValueDictionary)routeValues, method, (IDictionary<string, object>)new RouteValueDictionary(htmlAttributes));
        }

        public static string Theme(this UrlHelper helper, string relativeUrl)
        {
            var url = formatThemeResourceUrl(AdminService.Theme, relativeUrl);
            if(File.Exists(HttpContext.Current.Server.MapPath(url)))
                return url;
            else
                return formatThemeResourceUrl(AdminService.DefaultTheme, relativeUrl);
        }

        private static string formatThemeResourceUrl(string theme, string relativeUrl)
        {
            return String.Format(AdminService.VirtualThemeDir + theme + "/" + relativeUrl);
        }

        #region JQueryProxyScript
        public static string JQueryProxyScript<C>(this UrlHelper helper) where C : PublicPageControllerBase
        {
            return JQueryProxyScript<C>(helper, null);
        }

        public static string JQueryProxyScript<C>(this UrlHelper helper, string prefix) where C : PublicPageControllerBase
        {
            checkController<C>();

            string funcName = prefix ?? typeof(C).Name.Replace("Controller", "");
            var firstLetter = funcName.Substring(0, 1);
            funcName = firstLetter.ToUpper() + funcName.Substring(1);

            var variableName = firstLetter.ToLower() + funcName.Substring(1);

            var res = "function " + funcName + "(){\n";

            var functions = generateJQueryPostFunctions<C>(helper);

            foreach (var func in functions)
                res += func + "\n";

            res += "\n}\n" + variableName + " = new " + funcName + "();";
            return res;
        }

        private static void checkController<C>() where C : IController
        {
            var cType = typeof(C);

            bool isController = typeof(C).Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)
                && !cType.IsAbstract;
            if (!isController)
                throw new ArgumentException("C can't be abstract and its name must end with \"Controller\"", "C");
        }

//        private static IEnumerable<string> generateJQueryJsonFunctions<C>(UrlHelper helper, string atomeId) where C : IController
//        {
//            return from mi in typeof(C).GetMethods(System.Reflection.BindingFlags.Public | BindingFlags.Instance)
//                   where mi.ReturnType == typeof(JsonResult)
//                   select generateJQueryFunction(helper, null, mi, "getJSON", atomeId);
//        }

        private static IEnumerable<string> generateJQueryPostFunctions<C>(UrlHelper helper) where C : PublicPageControllerBase
        {
            return from mi in typeof(C).GetMethods(System.Reflection.BindingFlags.Public | BindingFlags.Instance)
                   let jsonResultType = typeof(JsonResult)
                   let AcceptVerbsAttributeType = typeof(AcceptVerbsAttribute)
                   let httpVersType = typeof(HttpVerbs)
                   where mi.ReturnType == jsonResultType ||
                   mi.GetCustomAttributes(AcceptVerbsAttributeType, false)
                     .Any(att => ((AcceptVerbsAttribute)att).Verbs
                       .Contains(Enum.GetName(httpVersType, HttpVerbs.Post).ToUpper()))
                   select generateJQueryFunction(helper, null, mi, "post", PublicPageControllerBase.GetAtome<C>().Id);
        }

        private static string generateJQueryFunction(UrlHelper helper, string prefix, MethodInfo method, string jqueryFunc, string atomeId)
        {
            var controllerName = method.DeclaringType.Name.Replace("Controller", "");
            var actionName = method.Name;
            var parameterNames = from p in method.GetParameters() select p.Name;
            var parameters = parameterNames.Aggregate("", (s, p) => s += p + ", ");

            var routeValues = new RouteValueDictionary();
            foreach (var param in parameterNames)
                routeValues.Add(param, "#" + param);
            routeValues.Add("atome", atomeId);
            var url = helper.Action(actionName, controllerName, routeValues).Replace("%23", "#");//remove # encoding, will be replaced by script at runtime.

            var code = "\n\t\tvar args = \"" + url + "\";\n";
            foreach (var param in parameterNames)
                code += "\t\targs = args.replace(\"#" + param + "\"," + param + ");\n";
            code += "\t\t$." + jqueryFunc + "(args, null, callback, \"json\");\n\t";
            return String.Format("\tthis.{0} = function({1}callback){{{2}}};", actionName, parameters, code);
        }
        #endregion

        public static string TreeList<T>(this HtmlHelper helper, IEnumerable<T> dataSource, Func<T, IEnumerable<T>> childSelector,
            Func<T, string> formatter)
        {
            return TreeList(helper, dataSource, childSelector, formatter, new RouteValueDictionary());
        }

        public static string TreeList<T>(this HtmlHelper helper, IEnumerable<T> dataSource, Func<T, IEnumerable<T>> childSelector,
            Func<T, string> formatter, IDictionary<string, object> htmlAttributes)
        {

            if (dataSource.Count() == 0)
                return "";

            var ulBuilder = new TagBuilder("ul");
            ulBuilder.MergeAttributes(htmlAttributes);

            var sb = new StringBuilder();
            foreach (var item in dataSource)
            {
                var liBuilder = new TagBuilder("li");
                liBuilder.InnerHtml = formatter(item) + TreeList(helper, childSelector(item), childSelector, formatter);
                sb.AppendLine(liBuilder.ToString(TagRenderMode.Normal));
            }

            ulBuilder.InnerHtml = sb.ToString();

            return ulBuilder.ToString(TagRenderMode.Normal);
        }
        
    }
}
