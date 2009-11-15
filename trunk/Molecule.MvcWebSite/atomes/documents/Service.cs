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
        private DirectoryInfo baseDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));

        private Service()
        {

        }

        private static Service instance { get { return Singleton<Service>.Instance; } }

        public static IFolderInfo GetRootFolder()
        {
            return new FolderInfo(instance.baseDir, instance.baseDir);
        }

        public static IEnumerable<IFolderInfo> GetFolders(string folderId)
        {
            return (GetFolder(folderId) as FolderInfo).GetFolders(instance.baseDir);
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

        public static IEnumerable<IDocumentInfo> GetDocuments(string folderId)
        {
            return (GetFolder(folderId) as FolderInfo).GetDocuments(instance.baseDir);
        }

        public static IEnumerable<IDocumentInfo> GetDocuments(IFolderInfo folderInfo)
        {
            return (folderInfo as FolderInfo).GetDocuments(instance.baseDir);
        }

       
    }


}
