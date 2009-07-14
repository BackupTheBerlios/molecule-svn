using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using Mono.Rocks;
using System.Web.Security;

namespace Molecule.WebSite.Services
{
    [Serializable]
    public class AtomeUserAuthorizations : List<AtomeUserAuthorizationItem>, IAtomeUserAuthorizations
    {
        const string anonymousUser = "anonymous";
        const bool defaultAuthorization = false;
        

        //only used directly by Serialization framework
        public AtomeUserAuthorizations()
        {
            
        }

        public AtomeUserAuthorizations(AtomeUserAuthorizations oldData)
        {
            var atomes = from atome in AtomeService.GetAtomes()
                         select atome.Name;

            var users = (from user in Membership.GetAllUsers().Cast<MembershipUser>()
                         orderby user.UserName select user.UserName)
                        .Concat(new string[] { anonymousUser });

            foreach(var atome in atomes)
            {
                var userAuths = new List<AtomeUserAuthorization>();
                foreach (var user in users)
                {
                    var auth = oldData != null ? oldData.TryGet(atome, user) : null;
                    if(auth == null)
                        auth = new AtomeUserAuthorization(atome, user, defaultAuthorization);
                    userAuths.Add(auth);
                }
                Add(new AtomeUserAuthorizationItem(){ Atome = atome, Authorizations = userAuths});
            }
        }

        public AtomeUserAuthorization Get(string atome, string user)
        {
            if (String.IsNullOrEmpty(user))
                user = anonymousUser;
            return this.First(t => t.Atome == atome).Authorizations.First(aua => aua.User == user);
        }


        public AtomeUserAuthorization TryGet(string atome, string user)
        {
            var item = this.FirstOrDefault(t => t.Atome == atome);
            if(item == null)
                return null;
            return item.Authorizations.FirstOrDefault(aua => aua.User == user);
        }

        public void Set(string atome, string user, bool value)
        {
            Get(atome, user).Authorized = value;
        }

        public void Set(AtomeUserAuthorization auth)
        {
            Set(auth.Atome, auth.User, auth.Authorized);
        }

        public static string AnonymousUser
        {
            get
            {
                return anonymousUser;
            }
        }
    }

    [Serializable]
    public class AtomeUserAuthorizationItem
    {
        public string Atome { get; set; }
        public List<AtomeUserAuthorization> Authorizations { get; set; }
    }

    [Serializable]
    public class AtomeUserAuthorization
    {
        public AtomeUserAuthorization()
        {
        }

        public AtomeUserAuthorization(string atome, string user, bool auth)
        {
            Atome = atome;
            User = user;
            Authorized = auth;
        }
        public string User { get; set; }
        public string Atome { get; set; }
        public bool Authorized { get; set; }
    }
}
