﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Reflection;

namespace Molecule.Web.Mvc
{
    public static class HelperExtensions
    {
        #region JQueryProxyScript
        public static string JQueryProxyScript<C>(this UrlHelper helper) where C : IController
        {
            return JQueryProxyScript<C>(helper, null);
        }

        public static string JQueryProxyScript<C>(this UrlHelper helper, string prefix) where C : IController
        {
            checkController<C>();

            string funcName = prefix ?? typeof(C).Name.Replace("Controller", "");
            var firstLetter = funcName.Substring(0, 1);
            funcName = firstLetter.ToUpper() + funcName.Substring(1);

            var variableName = firstLetter.ToLower() + funcName.Substring(1);

            var res = "function " + funcName + "(){\n";

            var functions = generateJQueryJsonFunctions<C>(helper).Concat(generateJQueryPostFunctions<C>(helper));

            foreach (var func in functions)
                res += func + "\n";

            res += "\n}\n" + variableName + " = new "+funcName+"();";
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

        private static IEnumerable<string> generateJQueryJsonFunctions<C>(UrlHelper helper) where C : IController
        {
            return from mi in typeof(C).GetMethods(System.Reflection.BindingFlags.Public | BindingFlags.Instance)
                              where mi.ReturnType == typeof(JsonResult)
                              select generateJQueryFunction(helper, null, mi, "getJSON");
        }

        private static IEnumerable<string> generateJQueryPostFunctions<C>(UrlHelper helper) where C : IController
        {
            return from mi in typeof(C).GetMethods(System.Reflection.BindingFlags.Public | BindingFlags.Instance)
                              where mi.GetCustomAttributes(typeof(AcceptVerbsAttribute), false)
                                .Any(att => ((AcceptVerbsAttribute)att).Verbs
                                  .Contains(Enum.GetName(typeof(HttpVerbs), HttpVerbs.Post).ToUpper()))
                              select generateJQueryFunction(helper, null, mi, "post");
        }

        private static string generateJQueryFunction(UrlHelper helper, string prefix, MethodInfo method, string jqueryFunc)
        {
            var controllerName = method.DeclaringType.Name.Replace("Controller","");
            var actionName = method.Name;
            var parameterNames = from p in method.GetParameters() select p.Name;
            var parameters = parameterNames.Aggregate("", (s, p) => s += p + ", ");

            bool hasParameters = parameterNames.Any();

            var routeValues = new RouteValueDictionary();
            foreach (var param in parameterNames)
                routeValues.Add(param, "#" + param);
            var url = helper.Action(actionName, controllerName, routeValues).Replace("%23", "#");//remove # encoding, will be replaced by script at runtime.

            var code = "\n\t\tvar args = \"" + url + "\";\n";
            foreach (var param in parameterNames)
                code += "\t\targs = args.replace(\"#" + param + "\"," + param + ");\n";
            code += "\t\t$."+jqueryFunc+"(args, callback);\n\t";
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
                sb.AppendLine(liBuilder.ToString( TagRenderMode.Normal));
            }

            ulBuilder.InnerHtml = sb.ToString();

            return ulBuilder.ToString(TagRenderMode.Normal);
        }
    }
}
