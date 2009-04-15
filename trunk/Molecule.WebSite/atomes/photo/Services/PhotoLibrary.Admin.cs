using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Molecule.Web;
using Molecule.Configuration;
using WebPhoto.Providers;
using Mono.Rocks;

namespace WebPhoto.Services
{
    public partial class PhotoLibrary
    {
        const string confTagUserAuthorizationsKey = "TagUserAuthorizations";
        const string confTagNameKey = "TagName";
        const string confNamespace = "WebPhoto.Admin";
        static TagName defaultTagName = TagName.Album;

        private TagUserAuthorizations tagUserAuthorizations;
        private object authLock = new object();

        public static TagUserAuthorizations TagUserAuthorizations
        {
            get
            {
                return Instance.tagUserAuthorizations;
            }
        }

        private void initAuthorizations()
        {
            var allTags = getAllTags(rootTags).ToList();
            Func<string, bool> tagExists = (id) => allTags.Any(t => t.Id == id);

            var oldData = ConfigurationClient.Get<TagUserAuthorizations>(
                confNamespace, confTagUserAuthorizationsKey, null);

            tagUserAuthorizations = new TagUserAuthorizations(oldData, tagExists);

            //save it again : can be updated regarding current provider and available tags.
            saveAuthorizations();
        }

        public static void SetTagUserAuthorization(string user, string tag, bool auth)
        {
            TagUserAuthorizations.Set(user, tag, auth);
        }

        protected void saveAuthorizations()
        {
            ConfigurationClient.Set(confNamespace, confTagUserAuthorizationsKey,
            tagUserAuthorizations);
        }

        public static void SaveTagUserAuthorizations()
        {
            Instance.saveAuthorizations();
            //authorization modified : need to refresh user librairies.
            Instance.buildUserLibraries();
        }

        public static IEnumerable<ITagInfo> AdminGetTagsByTag(string tagId)
        {
            return getAllTags(Instance.rootTags).First(t => t.Id == tagId)
                .ChildTags.Cast<ITagInfo>();
        }

        public static ITagInfo AdminGetTag(string tagId)
        {
            return getAllTags(Instance.rootTags).First(t => t.Id == tagId);
        }

        internal static IEnumerable<ITagInfo> AdminGetRootTags()
        {
            return Instance.rootTags.Cast<ITagInfo>();
        }

        TagName? tagName;

        public static TagName TagName
        {
            get
            {
                if (Instance.tagName == null)
                    Instance.tagName = ConfigurationClient.Get(confNamespace, confTagNameKey,
                        defaultTagName);

                return Instance.tagName.Value;
            }
            set
            {
                Instance.tagName = value;
                ConfigurationClient.Set(confNamespace, confTagNameKey, value);
            }
        }


        
    }
}
