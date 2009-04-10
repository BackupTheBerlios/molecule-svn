using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Molecule.Runtime;
using System.IO;

[assembly:PluginContainer]

namespace WebPhoto.Providers.FS
{
    [Plugin("File System")]
    public class PhotoLibraryProvider : IPhotoLibraryProvider
    {
        static string baseDir = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

        [IsUsablePlugin]
        public static bool IsUsable
        {
            get
            {
                return Directory.Exists(baseDir);
            }
        }

        #region IPhotoLibraryProvider Members

        public void Initialize()
        {
            
        }

        public IEnumerable<ITag> GetRootTags()
        {
            yield return new Tag(new DirectoryInfo(baseDir), null);
        }

        public IEnumerable<string> TagsRecentlyAdded
        { get { return null; } }

        #endregion

    }
}
