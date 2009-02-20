using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Molecule.IO
{
    public static class Extensions
    {
        //Original idea : http://www.extensionmethod.net/Details.aspx?ID=108
        public static void Create(this DirectoryInfo dirInfo, bool createParentDirectories)
        {
            if (!createParentDirectories)
                dirInfo.Create();
            else
            {
                if (dirInfo.Parent != null)
                    Create(dirInfo.Parent, true);
                if (!dirInfo.Exists)
                    dirInfo.Create();
            }
        }
    }
}
