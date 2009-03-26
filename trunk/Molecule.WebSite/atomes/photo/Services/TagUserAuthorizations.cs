using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Molecule.WebSite.Services;
using System.Web.Security;

namespace WebPhoto.Services
{
    [Serializable]
    public class TagUserAuthorizations : List<TagUserAuthorizationItem>
    {
        const bool defaultAuthorization = false;

        //only used directly by Serialization framework
        public TagUserAuthorizations()
        {
        }

        public TagUserAuthorizations(TagUserAuthorizations oldData, Func<string, bool> tagExist)
        {
            if (oldData == null)
                return;

            var users = (from user in Membership.GetAllUsers().Cast<MembershipUser>()
                         orderby user.UserName
                         select user.UserName)
                        .Concat(new string[] { AtomeUserAuthorizations.AnonymousUser });

            foreach (var item in oldData)
            {
                if (!tagExist(item.Tag))
                    continue;

                var userAuths = new List<TagUserAuthorization>();
                foreach (var user in users)
                {
                    var auth = oldData != null ? oldData.TryGet(item.Tag, user) : null;
                    if (auth == null)
                        auth = new TagUserAuthorization(item.Tag, user, defaultAuthorization);
                    userAuths.Add(auth);
                }
                item.Authorizations = userAuths;
                Add(item);
            }
        }

        public TagUserAuthorization Get(string tag, string user)
        {
            if (String.IsNullOrEmpty(user))
                user = AtomeUserAuthorizations.AnonymousUser;
            return this.First(t => t.Tag == tag).Authorizations.First(tua => tua.User == user);
        }

        public TagUserAuthorization TryGet(string tag, string user)
        {
            var item = this.FirstOrDefault(t => t.Tag == tag);
            if (item == null)
                return null;
            return item.Authorizations.FirstOrDefault(aua => aua.User == user);
        }

        public void Set(string tag, string user, bool value)
        {
            var item = this.FirstOrDefault(t => t.Tag == tag);
            if(item == null)
            {
                item = new TagUserAuthorizationItem(tag);
                this.Add(item);
            }
            var auth = item.Authorizations.FirstOrDefault(aua => aua.User == user);
            if(auth == null)
            {
                auth = new TagUserAuthorization(tag,user,value);
                item.Authorizations.Add(auth);
            }
            auth.Authorized = value;
        }

        public void Set(TagUserAuthorization auth)
        {
            Set(auth.Tag, auth.User, auth.Authorized);
        }
    }

    [Serializable]
    public class TagUserAuthorizationItem
    {
        public TagUserAuthorizationItem()
        {

        }

        public TagUserAuthorizationItem(string tag)
        {
            Tag = tag;
        }

        public string Tag { get; set; }
        public List<TagUserAuthorization> Authorizations { get; set; }
    }

    [Serializable]
    public class TagUserAuthorization
    {
        public TagUserAuthorization()
        {
        }

        public TagUserAuthorization(string tag, string user, bool auth)
        {
            Tag = tag;
            User = user;
            Authorized = auth;
        }
        public string User { get; set; }
        public string Tag { get; set; }
        public bool Authorized { get; set; }
    }
}
