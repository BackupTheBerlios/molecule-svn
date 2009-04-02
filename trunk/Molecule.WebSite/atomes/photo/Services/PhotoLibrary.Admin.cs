using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Molecule.Web;
using Molecule.Configuration;
using WebPhoto.Providers;

namespace WebPhoto.Services
{
    public partial class PhotoLibrary
    {
        const string confTagUserAuthorizationsKey = "TagUserAuthorizations";
        const string confNamespace = "WebPhoto.Admin";

        private TagUserAuthorizations tagUserAuthorizations;
        private object authLock = new object();

        public static TagUserAuthorizations TagUserAuthorizations
        {
            get
            {
                return instance.tagUserAuthorizations;
            }
        }

        private void initAuthorizations(Func<string, bool> tagExists)
        {
            lock (authLock)
            {
                var oldData = ConfigurationClient.Get<TagUserAuthorizations>(
                    confNamespace, confTagUserAuthorizationsKey, null);
                tagUserAuthorizations = new TagUserAuthorizations(oldData, tagExists);
                saveAuthorizations();
            }
        }

        public static void SetTagUserAuthorization(string user, string tag, bool auth)
        {
            lock (instance.authLock)
            {
                instance.tagUserAuthorizations.Set(user, tag, auth);
                instance.saveAuthorizations();
            }
        }

        protected void saveAuthorizations()
        {
            ConfigurationClient.Set(confNamespace, confTagUserAuthorizationsKey,
            tagUserAuthorizations);
        }

        public static void SaveTagUserAuthorizations()
        {
            instance.saveAuthorizations();
        }

        public static void UpdateTagUserAuthorizations()
        {
            lock (instance.authLock)
            {
                //reinit lazy loading.
                instance.initAuthorizations(t => GetTag(t) != null);
            }
        }

        public static IEnumerable<ITagInfo> AdminGetTagsByTag(string tagId)
        {
            return getAllTags(instance.rootTags).First(t => t.Id == tagId)
                .ChildTags.Cast<ITagInfo>();
        }

        //needed by preference page, can't be filtered by user authorizations.
        public static ITagInfo AdminGetTag(string tagId)
        {
            return getAllTags(instance.rootTags).First(t => t.Id == tagId);
        }

        internal static IEnumerable<ITagInfo> AdminGetRootTags()
        {
            return instance.rootTags.Cast<ITagInfo>();
        }
    }
}
