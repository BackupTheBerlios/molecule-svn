using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Molecule.WebSite.Services;
using System.Web.Routing;
using System.Globalization;
using System.Web.Compilation;
using System.Reflection;

namespace Molecule.MvcWebSite.Mvc
{
    public class ControllerFactory : IControllerFactory
    {
        IDictionary<IAtomeInfo, List<Type>> controllerTypes;
        NullAtomeInfo nullAtome = new NullAtomeInfo();

        object _lock = new object();

        //protected override Type GetControllerType(string controllerName)
        //{
        //    //if (!AtomeService.IsCurrentUserAuthorized())
        //        //return null;

        //    if(AtomeService.CurrentAtome != null)
        //        RequestContext.RouteData.DataTokens["Namespaces"] = AtomeService.CurrentAtome.ControllerNamespaces;

        //    //if(AtomeService.CurrentPathIsAtome)
        //    //    Console.WriteLine("ControllerFactory:" + AtomeService.CurrentAtome.Name);
        //    //Console.WriteLine("ControllerFactory:"+controllerName);
        //    //Console.WriteLine("ControllerFactory:" + HttpContext.Current.Request.Url);
        //}


        #region IControllerFactory Members

        public IController CreateController(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            if (requestContext == null)
                throw new ArgumentNullException("requestContext");

            if (String.IsNullOrEmpty(controllerName))
                throw new ArgumentException("Null or empty", "controllerName");

            if (controllerTypes == null)
                lock (_lock)
                    if (controllerTypes == null)
                        initControllerTypes();

            controllerName += "Controller";

            var ctrlType = controllerTypes[AtomeService.CurrentAtome ?? nullAtome]
                .FirstOrDefault(t => t.Name == controllerName);
            return (IController)Activator.CreateInstance(ctrlType);

        }

        private void initControllerTypes()
        {
            controllerTypes = AtomeService.GetAtomes().Concat(new List<IAtomeInfo>(){nullAtome})
                .ToDictionary(a => a, a => new List<Type>());
            foreach (var assembly in BuildManager.GetReferencedAssemblies().Cast<Assembly>())
                foreach (var controllerType in assembly.GetTypes().Where(IsControllerType))
                    controllerTypes[GetAtome(controllerType)].Add(controllerType);
        }

        private string GetName(Type t)
        {
            return t.Name.Substring(0, t.Name.Length - "Controller".Length);
        }

        private IAtomeInfo GetAtome(Type t)
        {
            return AtomeService.GetAtome(t.BaseType.GetGenericArguments().FirstOrDefault()) ?? nullAtome;
        }

        public void ReleaseController(IController controller)
        {
            var disposable = controller as IDisposable;
            if(disposable != null)
                disposable.Dispose();
        }

        #endregion

        internal static bool IsControllerType(Type t)
        {
            return
                t != null &&
                t.IsPublic &&
                t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase) &&
                !t.IsAbstract &&
                typeof(IController).IsAssignableFrom(t);
        }
    }

    public class NullAtomeInfo : IAtomeInfo
    {

        #region IAtomeInfo Members

        public string DefaultControllerName
        {
            get { throw new NotImplementedException(); }
        }

        public string PreferencesControllerName
        {
            get { throw new NotImplementedException(); }
        }

        public string Id
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public string Path
        {
            get { throw new NotImplementedException(); }
        }

        public bool HasPreferences
        {
            get { throw new NotImplementedException(); }
        }

        public bool AdminOnly
        {
            get { throw new NotImplementedException(); }
        }

        public string ClassName
        {
            get { throw new NotImplementedException(); }
        }

        public void RegisterRoutes(RouteCollection routes)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
