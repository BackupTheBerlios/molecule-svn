using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Molecule.MvcWebSite.Models;

namespace Molecule.MvcWebSite.atomes.music.Data
{
    public class AdminData : ProviderSelectorData
    {
        public bool LastfmEnabled { get; set; }
        public string LastfmUsername { get; set; }
        public string LastfmPassword { get; set; }
    }
}
