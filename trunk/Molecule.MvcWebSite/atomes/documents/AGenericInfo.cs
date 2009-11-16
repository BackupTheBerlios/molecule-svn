using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Molecule.Atomes.Documents
{
    public abstract class AGenericInfo : IGenericInfo
    {
        public AGenericInfo(string id)
        {
            this.Id = id;
        }

        public AGenericInfo(FileSystemInfo fsi, DirectoryInfo baseDir)
        {
            Id = fsi.FullName.Remove(0, baseDir.FullName.Length).TrimStart('/', '\\');
        }

        #region IGenericInfo Members

        public string Id
        {
            get;
            private set;
        }

        public string Name
        {
            get { return Info.Name; }
        }

        public string Path
        {
            get { return Info.FullName; }
        }

        #endregion

        protected abstract System.IO.FileSystemInfo Info
        {
            get;
        }

        protected static void CheckParent(DirectoryInfo item, DirectoryInfo baseDir)
        {
            var parent = item;
            while (parent != null) {
                if (parent.FullName == baseDir.FullName)
                    return;
                parent = parent.Parent;
            }
            throw new UnauthorizedAccessException();
        }
    }
}
