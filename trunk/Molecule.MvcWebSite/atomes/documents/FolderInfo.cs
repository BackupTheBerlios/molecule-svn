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

        public FolderInfo(DirectoryInfo di, DirectoryInfo baseDir)
            : base(di, baseDir)
        {
            this.di = di;
        }

        public FolderInfo(string id, DirectoryInfo baseDir)
            : base(id)
        {
            di = new DirectoryInfo(System.IO.Path.Combine(baseDir.FullName, id));
            CheckParent(di, baseDir);
        }

        
        protected override FileSystemInfo Info
        {
            get { return di; }
        }

        public IEnumerable<IFolderInfo> GetFolders(DirectoryInfo baseDir)
        {
            return from dir in di.GetDirectories()
                   select new FolderInfo(dir, baseDir) as IFolderInfo;
        }

        public IEnumerable<IDocumentInfo> GetDocuments(DirectoryInfo baseDir)
        {
            return from file in di.GetFiles()
                   select new DocumentInfo(file, baseDir) as IDocumentInfo;
        }

        public FolderInfo GetParent(DirectoryInfo baseDir)
        {
            return new FolderInfo(di.Parent, baseDir);
        }

        public void Create()
        {
            if (!di.Exists)
                di.Create();
        }

        public void Delete()
        {
            if (di.Exists)
                di.Delete(true);
        }

        public FolderInfo CreateSubdirectory(string name, DirectoryInfo baseDir)
        {
            return new FolderInfo(di.CreateSubdirectory(name), baseDir);
        }
    }
}
