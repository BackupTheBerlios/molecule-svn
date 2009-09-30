using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Molecule.MvcWebSite.Controllers;
using Microsoft.Web.Mvc.Internal;
using System.Linq.Expressions;

namespace Molecule.MvcWebSite.Mvc
{
    public static class UrlHelperExtensions
    {
        public static string Action<T>(this UrlHelper helper, Expression<Action<T>> action)
            where T : PublicPageControllerBase
        {
            var routeValues = ExpressionHelper.GetRouteValuesFromExpression(action);
            return helper.RouteUrl(routeValues);
        }

        public static string Action<T>(this UrlHelper helper, Expression<Action<T>> action, string atomeId)
            where T : PublicPageControllerBase
        {
            var routeValues = ExpressionHelper.GetRouteValuesFromExpression(action);
            if (!String.IsNullOrEmpty(atomeId))
                routeValues.Add("atome", atomeId);
            return helper.RouteUrl(routeValues);
        }
    }
}
