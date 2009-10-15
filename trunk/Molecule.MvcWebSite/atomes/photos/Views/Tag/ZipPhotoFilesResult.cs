using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using WebPhoto.Services;
using WebPhoto.Providers;
using ICSharpCode.SharpZipLib.Zip;

namespace Molecule.MvcWebSite.atomes.photos.Views
{
    public class ZipPhotoFilesResult : ActionResult
    {

        public ITagInfo Tag { get; set; }

        public IEnumerable<IPhotoInfo> Photos { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            string fileName = Path.GetTempFileName();

            var response = context.HttpContext.Response;

            using (var zipOutputStream = new ZipOutputStream(new FileStream(fileName, FileMode.OpenOrCreate)))
            {
                zipOutputStream.SetLevel(0);
                foreach (var photo in Photos)
                {
                    //FileInfo fileInfo = new FileInfo(photo.MediaFilePath);
                    ZipEntry entry = new ZipEntry(Tag.Name + @"\" + photo.Id + ".jpg");
                    zipOutputStream.PutNextEntry(entry);
                    using (FileStream fs = System.IO.File.OpenRead(photo.MediaFilePath))
                    {

                        byte[] buff = new byte[1024];
                        int n = 0;
                        while ((n = fs.Read(buff, 0, buff.Length)) > 0)
                            zipOutputStream.Write(buff, 0, n);
                    }
                }
                zipOutputStream.Finish();
            }

            System.IO.FileInfo file = new System.IO.FileInfo(fileName);
            response.Clear();
            response.AddHeader("Content-Disposition", "attachment; filename=" + "Photos.zip");
            response.AddHeader("Content-Length", file.Length.ToString());
            response.ContentType = "application/octet-stream";
            response.WriteFile(file.FullName);
            response.End();
            System.IO.File.Delete(fileName);
        }

    }
}
