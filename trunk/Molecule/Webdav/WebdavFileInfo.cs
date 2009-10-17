using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Molecule.Webdav
{
    public class WebdavFileInfo
    {
        //   LastAccessTime = fi.LastAccessTime, FileName = fi.FullName, Size = fi.Length
        public DateTime LastAccessTime
        {
            get;
            set;
        }

        public string FileName
        {

            get;
            set;
        }

        public long Size
        {
            get;
            set;
        }


        public bool IsFile
        {
            get;
            set;
        }

    }
}
