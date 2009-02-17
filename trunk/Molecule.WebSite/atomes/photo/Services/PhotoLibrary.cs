using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Molecule.Web;
using WebPhoto.Providers;
using Molecule.Runtime;

namespace WebPhoto.Services
{
    public class PhotoLibrary
    {
        static object instanceLock = new object();

        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(PhotoLibrary));

        static string providerName;

        static PhotoLibrary()
        {
            providerName = Molecule.Configuration.ConfigurationClient.Client.Get<string>("WebPhoto", "LibraryProvider", "Stub");
        }

        static PhotoLibrary instance
        {
            get
            {
                return Singleton<PhotoLibrary>.Instance;
            }
        }

        private PhotoLibrary()
        {
            UpdateProvider();
        }

        private void UpdateProvider()
        {
            throw new NotImplementedException();
        }

        public static string CurrentProvider
        {
            get { return providerName; }
            set
            {
                providerName = value;
                Molecule.Configuration.ConfigurationClient.Client.Set<string>("WebPhoto", "LibraryProvider", value);
                instance.UpdateProvider();
            }
        }

        public static IEnumerable<ProviderInfo> Providers
        {
            get
            {
                foreach (var provider in Plugin<IPhotoLibraryProvider>.List(providerDirectory))
                    yield return new ProviderInfo() { Description = provider.Description, Name = provider.Name };
            }
        }

        static string providerDirectory
        {
            get
            {
                return HttpContext.Current.Server.MapPath("~/atomes/webphoto/bin/providers");
            }
        }

    }
}
