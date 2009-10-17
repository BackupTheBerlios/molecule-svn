using System.Web.Routing;
using System.Web.Mvc;

namespace Molecule.MvcWebSite.RouteHandlers
{
    public class WebdavRouteHandler : IRouteHandler
    {
        #region IRouteHandler Members

        // Needed to handle long path.
        // We will never handle actions.
        public System.Web.IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            string path = requestContext.RouteData.Values["path"] as string;
            if (path != null)
            {
                int idx = path.LastIndexOf('/');
                if (idx >= 0)
                {
                    string actionName = path.Substring(idx + 1);
                    if (actionName.Length > 0)
                    {
                        // requestContext.RouteData.Values["action"] = actionName; 
                        // requestContext.RouteData.Values["path"] = path.Substring(0, idx);
                        requestContext.RouteData.Values["path"] = path;
                    }
                }
            } return new MvcHandler(requestContext);
        }

        #endregion
    }

}