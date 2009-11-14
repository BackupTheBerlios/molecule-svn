using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Molecule.Atomes.Documents
{
    public class DocumentInfo : AGenericInfo, IDocumentInfo
    {
        FileInfo fi;

        public DocumentInfo(string id, DirectoryInfo baseDir)
        {
            base.Id = id;
            fi = new FileInfo(id.UnZip());
            CheckParent(fi.Directory, baseDir);
        }

        public DocumentInfo(FileInfo fi)
        {
            this.fi = fi;
        }

        protected override FileSystemInfo Info
        {
            get { return fi; }
        }
    }
}
