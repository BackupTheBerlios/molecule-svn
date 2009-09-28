using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Molecule.MvcWebSite.Models;
using WebPhoto.Services;
using Mono.Rocks;
using WebPhoto.Providers;

namespace Molecule.MvcWebSite.atomes.photos.Data
{
    public class TagData
    {
        ITagInfo tagInfo;

        public string Id { get { return tagInfo.Id; } }
        public string Name { get { return tagInfo.Name; } }
        public bool Selected { get { return PhotoLibrary.TagUserAuthorizations.ContainsTag(tagInfo.Id); } }

        public TagData(ITagInfo tagInfo)
        {
            this.tagInfo = tagInfo;
        }
        public IEnumerable<TagData> Tags
        {
            get
            {
                return from tag in PhotoLibrary.AdminGetTagsByTag(tagInfo.Id)
                       select new TagData(tag);
            }
        }
    }

    public class TagUserAuthorizationItemData
    {
        TagUserAuthorizationItem tuai;
        public TagUserAuthorizationItemData(TagUserAuthorizationItem tuai)
        {
            this.tuai = tuai;
        }
        public string TagName { get { return PhotoLibrary.AdminGetTag(tuai.TagId).Name; } }
        public IEnumerable<TagUserAuthorizationData> Authorizations
        {
            get
            {
                return from tua in tuai.Authorizations select new TagUserAuthorizationData(tua);
            }
        }
    }

    public class TagUserAuthorizationData
    {
        TagUserAuthorization tua;
        public TagUserAuthorizationData(TagUserAuthorization tua)
        {
            this.tua = tua;
        }
        public string Value { get { return GetValue(tua.TagId, tua.User); } }
        public bool Authorized { get { return tua.Authorized; } }

        public static string GetValue(string tagId, string user)
        {
            return tagId + "," + user;
        }
    }

    public class AdminData : ProviderSelectorData
    {
        public IEnumerable<KeyValuePair<TagName, string>> TagNames { get; set; }
        public KeyValuePair<TagName, string> SelectedTagName { get; set; }
        public IEnumerable<string> UserNames { get; set; }
        public IEnumerable<TagUserAuthorizationItemData> TagUserAuthorizations { get; set; }
        public IEnumerable<TagData> RootTags { get; set; }
    }
}
