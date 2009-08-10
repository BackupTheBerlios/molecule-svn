using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Molecule.Web.Mvc
{
    public static class HelperExtensions
    {

        public static string JQueryActionScript<C>(this UrlHelper helper, Expression<Func<C, JsonResult>> action)
            where C : IController
        {
            return JQueryActionScript<C>(helper, action, null);
        }

        public static string JQueryActionScript<C>(this UrlHelper helper, Expression<Func<C, JsonResult>> action, string prefix)
            where C : IController
        {
            var cType = typeof(C);
            
            bool isController = cType.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)
                && !cType.IsAbstract;
            if (!isController)
                throw new ArgumentException("C can't be abstract and its name must end with \"Controller\"", "C");

            var controllerName = cType.Name.Replace("Controller", "");

            var expr = action.Body as MethodCallExpression;
            if (expr == null)
                throw new ArgumentException("action must be a method controller call.");

            var actionName = expr.Method.Name;

            var parameterNames = from p in expr.Method.GetParameters() select p.Name;
            var parameters = parameterNames.Aggregate("", (s, p) => s += p+", ");
            
            bool hasParameters = parameterNames.Any();

            var routeValues = new RouteValueDictionary();
            foreach (var param in parameterNames)
                routeValues.Add(param, "#"+param);
            var url = helper.Action(actionName, controllerName, routeValues);
            if(hasParameters)
                url += "";
            var code = "\n\tvar args = \""+url+"\";\n";
            foreach (var param in parameterNames)
                code += "\targs = args.replace(\"#" + param + "\"," + param + ");\n";
            code += "\t$.getJSON(args, callback);\n";
            return String.Format("function {0}{1}({2}callback){{{3}}}",prefix??(controllerName+"_"), actionName, parameters, code);
        }
    }
}
