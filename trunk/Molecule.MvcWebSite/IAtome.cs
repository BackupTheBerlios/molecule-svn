using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace Molecule.MvcWebSite
{
    public interface IAtome
    {
        void RegisterRoutes(RouteCollection routes);
        Type PreferencesController { get; }
        IEnumerable<string> ControllerNamespaces { get; }
    }
}
