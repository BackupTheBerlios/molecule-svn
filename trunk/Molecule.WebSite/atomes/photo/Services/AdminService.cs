using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Molecule.Web;
using Molecule.Configuration;

namespace WebPhoto.Services
{
    public class AdminService
    {
        const string confTagUserAuthorizationsKey = "TagUserAuthorizations";
        const string confNamespace = "WebPhoto.Admin";

        private TagUserAuthorizations tagUserAuthorizations;
        private object authLock = new object();

        private static AdminService instance { get { return Singleton<AdminService>.Instance; } }

        public static TagUserAuthorizations TagUserAuthorizations
        {
            get
            {
                if (instance.tagUserAuthorizations == null)
                    instance.initAuthorizations();

                return instance.tagUserAuthorizations;
            }
        }

        private void initAuthorizations()
        {
            lock (authLock)
            {
                if (tagUserAuthorizations == null)
                {
                    var oldData = ConfigurationClient.Get<TagUserAuthorizations>(
                        confNamespace, confTagUserAuthorizationsKey, null);
                    tagUserAuthorizations = new TagUserAuthorizations(oldData,
                        tagId => PhotoLibrary.GetTag(tagId) != null);
                }
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
                instance.tagUserAuthorizations = null;
            }
        }
    }
}
