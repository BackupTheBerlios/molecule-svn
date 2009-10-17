using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Molecule.Webdav
{
    public interface IVirtualWebdavFolder
    {
        string RootDirectoryName { get; }

        IEnumerable<WebdavFileInfo> List(string path);

        byte[] GetFile(string path);
    }
}
