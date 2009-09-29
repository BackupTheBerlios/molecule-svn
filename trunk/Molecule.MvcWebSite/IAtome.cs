using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace Molecule.MvcWebSite
{
    public interface IAtome
    {
        Type PreferencesController { get; }
        Type DefaultController { get; }
        IEnumerable<string> ControllerNamespaces { get; }
        bool AdminOnly { get; }
        string Name { get; }
        
    }
}
