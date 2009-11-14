using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Molecule.Atomes.Documents
{
    public class FolderInfo : AGenericInfo, IFolderInfo
    {
        DirectoryInfo di;
        string id;


        public FolderInfo(DirectoryInfo di)
        {
            this.di = di;
        }

        public FolderInfo(string id, DirectoryInfo baseDir)
        {
            base.Id = id;
            di = new DirectoryInfo(id.UnZip());
            CheckParent(di, baseDir);
        }

        
        protected override FileSystemInfo Info
        {
            get { return di; }
        }

        public IEnumerable<IFolderInfo> GetFolders()
        {
            return from dir in di.GetDirectories()
                   select new FolderInfo(dir) as IFolderInfo;
        }

        public IEnumerable<IDocumentInfo> GetDocuments()
        {
            return from file in di.GetFiles()
                   select new DocumentInfo(file) as IDocumentInfo;
        }
    }
}
