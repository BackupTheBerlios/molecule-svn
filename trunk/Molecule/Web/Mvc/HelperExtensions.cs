using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Web.Mvc.Html;

namespace Molecule.Web.Mvc
{
    public static class HelperExtensions
    {
        public static string JQueryActionScript<C>(this UrlHelper helper, Expression<Func<C, JsonResult>> action)
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
            var parameters = parameterNames.Aggregate("", (s, p) => s += (s != "" ? ", " : "") + p);
            
            bool hasParameters = parameterNames.Any();
            var url = helper.Action(actionName, controllerName).TrimEnd('/');
            if(hasParameters)
                url += "?";
            var code = "return $.getJSON(\"" + url +
                parameterNames.Aggregate("", (s, p) => s += (s != "" ? "+\"&" : "") + p + "=\"+" + p)
                + (!hasParameters ? "\"" : "") + ");";
            return String.Format("function {0}_{1}({2}){{{3}}}", controllerName, actionName, parameters, code);
        }
    }
}
