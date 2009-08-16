using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Molecule.Atome;

namespace Molecule.MvcWebSite.Models
{
    public class ProviderSelectorData
    {
        public IEnumerable<ProviderInfo> Providers { get; set; }
    }
}
