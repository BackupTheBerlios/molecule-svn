using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Molecule.Web;
using System.IO;

namespace Molecule.Atomes.Documents
{
    public class Service
    {
        DirectoryInfo baseDir = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.MyDocuments), "Molecule"));

        private Service()
        {
            if (!baseDir.Exists)
                baseDir.Create();
        }

        private static Service instance { get { return Singleton<Service>.Instance; } }

        public static IFolderInfo GetRootFolder()
        {
            return new FolderInfo(instance.baseDir, instance.baseDir);
        }

        public static IEnumerable<IFolderInfo> GetFolders(IFolderInfo folderInfo)
        {
            return (folderInfo as FolderInfo).GetFolders(instance.baseDir);
        }

        public static IFolderInfo GetFolder(string id)
        {
            return new FolderInfo(id, instance.baseDir);
        }

        public static IDocumentInfo GetDocument(string id)
        {
            return new DocumentInfo(id, instance.baseDir);
        }

        public static IEnumerable<IDocumentInfo> GetDocuments(IFolderInfo folderInfo)
        {
            return (folderInfo as FolderInfo).GetDocuments(instance.baseDir);
        }

        public static void Create(IFolderInfo folderInfo)
        {
            (folderInfo as FolderInfo).Create();
        }

        public static IFolderInfo CreateSubdirectory(string folderId, string name)
        {
            return (GetFolder(folderId) as FolderInfo).CreateSubdirectory(name, instance.baseDir);
        }

        public static IFolderInfo Delete(string folderId)
        {
            if (String.IsNullOrEmpty(folderId))
                throw new ApplicationException("Can't delete base folder.");

            var folderInfo = GetFolder(folderId) as FolderInfo;
            var parent = folderInfo.GetParent(instance.baseDir);
            folderInfo.Delete();
            return parent;
        }
    }
}
