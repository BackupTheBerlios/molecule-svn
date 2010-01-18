using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Molecule.WebSite.Services;

namespace Molecule.MvcWebSite.atomes.admin.Data
{
    public class PreferencesData
    {
        public IEnumerable<DeletableUserData> DeletableUsers { get; set; }
        public IEnumerable<string> AuthorizableUsers { get; set; }
        public IEnumerable<AtomeUserAuthorizationsData> Authorizations { get; set; }
        public string Title { get; set; }
        public string SelectedTheme { get; set; }
        public IEnumerable<string> Themes { get; set; }
    }

    public class DeletableUserData
    {
        public string Name{get;set;}
        public DateTime LastLoginDate{get;set;}
    }

    public class AtomeUserAuthorizationsData
    {
        string atome;

        public AtomeUserAuthorizationsData(string atome, IEnumerable<string> users)
        {
            this.atome = atome;
            this.Authorizations = from user in users
                                  select new AtomeUserAuthorizationData( atome, user,
                                      AdminService.AtomeUserAuthorizations.IsAuthorized(atome, user));
        }

        public IEnumerable<AtomeUserAuthorizationData> Authorizations
        {
            get;
            private set;
        }

        public string Atome { get { return atome; } }
    }

    public class AtomeUserAuthorizationData
    {
        public AtomeUserAuthorizationData(string atome, string user, bool authorized)
        {
            this.Value = GetValue(atome, user);
            this.Authorized = authorized;
        }

        public string Value { get; private set; }

        public bool Authorized { get; private set; }

        public static string GetValue(string atome, string user)
        {
            return atome + "," + user;
        }

    }
}
