using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Globalization;

namespace Molecule.MvcWebSite.Mvc
{
    public class ViewEngine : System.Web.Mvc.WebFormViewEngine
    {
        private const string _cacheKeyFormat = ":ViewCacheEntry:{0}:{1}:{2}:{3}:";
        private const string _cacheKeyPrefix_Master = "Master";
        private const string _cacheKeyPrefix_Partial = "Partial";
        private const string _cacheKeyPrefix_View = "View";
        private static readonly string[] _emptyLocations = new string[0];

        public ViewEngine()
        {
            base.ViewLocationFormats = new string[] { 
                "~/atomes/{2}/Views/{1}/{0}.aspx", 
                "~/atomes/{2}/Views/{1}/{0}.ascx", 
                "~/Views/{1}/{0}.aspx", 
                "~/Views/{1}/{0}.ascx", 
                "~/Views/Shared/{0}.aspx", 
                "~/Views/Shared/{0}.ascx" 
                };

            base.PartialViewLocationFormats = ViewLocationFormats;
            base.MasterLocationFormats = ViewLocationFormats;
        }



        private VirtualPathProvider _vpp;


        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (String.IsNullOrEmpty(viewName))
            {
                throw new ArgumentException("viewName");
            }

            string[] viewLocationsSearched;
            string[] masterLocationsSearched;

            string controllerName = controllerContext.RouteData.GetRequiredString("controller");
            string viewPath = GetPath(controllerContext, ViewLocationFormats, "ViewLocationFormats", viewName, controllerName, _cacheKeyPrefix_View, useCache, out viewLocationsSearched);
            string masterPath = GetPath(controllerContext, MasterLocationFormats, "MasterLocationFormats", masterName, controllerName, _cacheKeyPrefix_Master, useCache, out masterLocationsSearched);

            if (String.IsNullOrEmpty(viewPath) || (String.IsNullOrEmpty(masterPath) && !String.IsNullOrEmpty(masterName)))
            {
                return new ViewEngineResult(viewLocationsSearched.Union(masterLocationsSearched));
            }

            return new ViewEngineResult(CreateView(controllerContext, viewPath, masterPath), this);
        }
        private string GetPath(ControllerContext controllerContext, string[] locations, string locationsPropertyName, string name, string controllerName, string cacheKeyPrefix, bool useCache, out string[] searchedLocations)
        {
            searchedLocations = _emptyLocations;

            if (String.IsNullOrEmpty(name))
            {
                return String.Empty;
            }

            if (locations == null || locations.Length == 0)
            {
                throw new InvalidOperationException();
            }

            bool nameRepresentsPath = IsSpecificPath(name);
            string cacheKey = CreateCacheKey(cacheKeyPrefix, name, (nameRepresentsPath) ? String.Empty : controllerName);

            if (useCache)
            {
                string result = ViewLocationCache.GetViewLocation(controllerContext.HttpContext, cacheKey);
                if (result != null)
                {
                    return result;
                }
            }

            return (nameRepresentsPath) ?
            GetPathFromSpecificName(controllerContext, name, cacheKey, ref searchedLocations) :
            GetPathFromGeneralName(controllerContext, locations, name, controllerName, cacheKey, ref searchedLocations);
        }

        private string GetPathFromGeneralName(ControllerContext controllerContext, string[] locations, string name, string controllerName, string cacheKey, ref string[] searchedLocations)
        {
            var result = String.Empty;
            searchedLocations = new string[locations.Length];
            var atome = controllerContext.RouteData.Values["atome"].ToString();

            for (int i = 0; i < locations.Length; i++)
            {
                string virtualPath = String.Format(CultureInfo.InvariantCulture, locations[i], name, controllerName, atome);

                if (FileExists(controllerContext, virtualPath))
                {
                    searchedLocations = _emptyLocations;
                    result = virtualPath;
                    ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, result);
                    break;
                }

                searchedLocations[i] = virtualPath;
            }

            return result;
        }

        private string CreateCacheKey(string prefix, string name, string controllerName)
        {
            return String.Format(CultureInfo.InvariantCulture, _cacheKeyFormat,
            GetType().AssemblyQualifiedName, prefix, name, controllerName);
        }

        private string GetPathFromSpecificName(ControllerContext controllerContext, string name, string cacheKey, ref string[] searchedLocations)
        {
            string result = name;

            if (!FileExists(controllerContext, name))
            {
                result = String.Empty;
                searchedLocations = new[] { name };
            }

            ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, result);
            return result;
        }

        private static bool IsSpecificPath(string name)
        {
            char c = name[0];
            return (c == '~' || c == '/');
        }

    }
}

