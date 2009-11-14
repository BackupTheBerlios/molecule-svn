using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Molecule.Atomes.Documents
{
    public abstract class AGenericInfo : IGenericInfo
    {
        #region IGenericInfo Members

        string id;

        public string Id
        {
            get
            {
                if (String.IsNullOrEmpty(id))
                    id = Info.FullName.Zip();
                return id;
            }
            protected set {
                id = value;
            }
        }

        public string Name
        {
            get { return Info.Name; }
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
                if (parent == baseDir)
                    return;
                parent = parent.Parent;
            }
            throw new UnauthorizedAccessException();
        }
    }
}
