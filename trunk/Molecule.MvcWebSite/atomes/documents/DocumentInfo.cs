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
            : base(id)
        {
            fi = new FileInfo(System.IO.Path.Combine(baseDir.FullName,id));
            CheckParent(fi.Directory, baseDir);
        }

        public DocumentInfo(FileInfo fi, DirectoryInfo baseDir)
            : base(fi, baseDir)
        {
            this.fi = fi;
        }

        protected override FileSystemInfo Info
        {
            get { return fi; }
        }

        public void Delete()
        {
            if (fi.Exists)
                fi.Delete();
        }
    }
}
