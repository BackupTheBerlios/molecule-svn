using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Molecule.Webdav
{
    public class VirtualWebdavFolderService
    {
        Dictionary<string, IVirtualWebdavFolder> fileProviders;

        private VirtualWebdavFolderService()
        {
            this.fileProviders = new Dictionary<string, IVirtualWebdavFolder>();
        }

        public void RegisterVirtualWebdavFolder(IVirtualWebdavFolder fileProvider)
        {
            this.fileProviders.Add(fileProvider.RootDirectoryName, fileProvider);
        }

        public IVirtualWebdavFolder GetVirtualWebdavFolder(string name)
        {
            if (fileProviders.ContainsKey(name))
            {
                return fileProviders[name];
            }
            return null;
        }

        public IEnumerable<string> GetVirtualWebdavFolderRootsName()
        {
            return from f in fileProviders select f.Key;

        }

    }
}
