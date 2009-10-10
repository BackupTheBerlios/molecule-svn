using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Molecule.WebSite.Services;
using System.Web.Security;
using System.Xml.Serialization;
using Molecule;

namespace WebPhoto.Services
{
    [Serializable]
    public class TagUserAuthorizations : List<TagUserAuthorizationItem>
    {
        public const bool defaultAuthorization = false;

        //only used directly by Serialization framework
        public TagUserAuthorizations()
        {
            users = (from user in Membership.GetAllUsers().Cast<MembershipUser>()
                     orderby user.UserName
                     select user.UserName)
                        .Concat(new string[] { AtomeUserAuthorizations.AnonymousUser });
        }

        IEnumerable<string> users;

        public TagUserAuthorizations(TagUserAuthorizations oldData, Func<string, bool> tagExist)
            :this()
        {
            if (oldData == null)
                return;

            foreach (var item in oldData)
            {
                if (!tagExist(item.TagId))
                    continue;

                var userAuths = new List<TagUserAuthorization>();
                foreach (var user in users)
                {
                    var auth = oldData.NotNull( o => o.TryGet(item.TagId, user));
                    if (auth == null)
                        auth = new TagUserAuthorization(item.TagId, user, defaultAuthorization);
                    userAuths.Add(auth);
                }
                item.Authorizations = userAuths;
                Add(item);
            }
        }

        public TagUserAuthorization Get(string tagId, string user)
        {
            if (String.IsNullOrEmpty(user))
                user = AtomeUserAuthorizations.AnonymousUser;
            return this.First(t => t.TagId == tagId).Authorizations.First(tua => tua.User == user);
        }

        public TagUserAuthorization TryGet(string tagId, string user)
        {
            var item = this.FirstOrDefault(t => t.TagId == tagId);
            if (item == null)
                return null;
            return item.Authorizations.FirstOrDefault(aua => aua.User == user);
        }

        public void Set(string tagId, string user, bool value)
        {
            var item = this.FirstOrDefault(t => t.TagId == tagId);
            if(item == null)
            {
                item = new TagUserAuthorizationItem(tagId, users);
                this.Add(item);
            }
            var auth = item.Authorizations.FirstOrDefault(aua => aua.User == user);
            if(auth == null)
            {
                auth = new TagUserAuthorization(tagId,user,value);
                item.Authorizations.Add(auth);
            }
            auth.Authorized = value;
        }

        public void Set(TagUserAuthorization auth)
        {
            Set(auth.TagId, auth.User, auth.Authorized);
        }

        public void AddTag(string tagId)
        {
            this.Add(new TagUserAuthorizationItem(tagId, users));
        }

        public void RemoveTag(string tagId)
        {
            this.Remove(this.First(tua => tua.TagId == tagId));
        }

        public bool ContainsTag(string tagId)
        {
            return this.Any(tua => tua.TagId == tagId);
        }

        public IEnumerable<string> GetTags(string user)
        {
            return from item in this
                   where item.Authorizations.Any(tua => tua.Authorized && tua.User == user)
                   select item.TagId;
        }
    }

    [Serializable]
    public class TagUserAuthorizationItem
    {
        public TagUserAuthorizationItem()
        {
            
        }

        public TagUserAuthorizationItem(string tagId, IEnumerable<string> users)
        {
            TagId = tagId;
            Authorizations = new List<TagUserAuthorization>();
            foreach (string user in users)
            {
                var auth = new TagUserAuthorization(tagId, user, TagUserAuthorizations.defaultAuthorization);
                Authorizations.Add(auth);
            }
        }

        public string TagId { get; set; }
        public List<TagUserAuthorization> Authorizations { get; set; }
    }

    [Serializable]
    public class TagUserAuthorization
    {
        public TagUserAuthorization()
        {
        }

        public TagUserAuthorization(string tagId, string user, bool auth)
        {
            TagId = tagId;
            User = user;
            Authorized = auth;
        }
        public string User { get; set; }
        public string TagId { get; set; }
        public bool Authorized { get; set; }
    }
}
