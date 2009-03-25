using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Molecule.Web;
using Molecule.IO;
using System.IO;

namespace Upload
{
    public class UploadService
    {
        private const string destinationPathKey = "DestinationPath";
        string destinationPath;
    
        private UploadService()
        {

            string defaultdownloadPath = XdgBaseDirectorySpec.GetUserDirectory("XDG_DOWNLOAD_DIR", "Downloads");
            if (!Directory.Exists(defaultdownloadPath))
            {
                Directory.CreateDirectory(defaultdownloadPath);
            }

            destinationPath = Molecule.Configuration.ConfigurationClient.Client.Get<string>(ConfigurationNamespace, destinationPathKey,
                                                                                            defaultdownloadPath);
        }

        static UploadService instance
        {
            get
            {
                return Singleton<UploadService>.Instance;
            }
        }

        public static string ConfigurationNamespace
        {
            get { return "Upload"; }
        }

        public static string DestinationPath
        {
            get
            {
                return instance.destinationPath;
            }
            set
            {
                instance.destinationPath = value;
                Molecule.Configuration.ConfigurationClient.Client.Set<string>(ConfigurationNamespace, destinationPathKey, value);
            }
        }


    }
}
