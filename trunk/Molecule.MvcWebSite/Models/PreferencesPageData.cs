using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Molecule.WebSite.Services;

namespace Molecule.MvcWebSite.Models
{
    public class PreferencesPageData
    {
        public IEnumerable<IAtomeInfo> Atomes { get; set; }
    }
}
