using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Molecule.MvcWebSite.atomes.admin.Data
{
    public class PreferencesData
    {
        public IEnumerable<MembershipUser> Users { get; set; }
    }
}
