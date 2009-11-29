using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Molecule.MvcWebSite.atomes.music.Views.Player
{
    public class SecureFileResult : FileResult
    {

        public SecureFileResult(string fileName, string contentType)
            : base(contentType)
        {
            FileName = fileName;
        }

        public string FileName
        {
            get;
            private set;
        }

        protected override void WriteFile(HttpResponseBase response)
        {
            response.WriteFile(FileName, true);
        }

    }

}
