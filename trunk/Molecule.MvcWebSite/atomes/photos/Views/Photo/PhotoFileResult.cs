using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mono.Rocks;
using WebPhoto.Services;
using System.IO;
using WebPhoto.Providers;

namespace Molecule.MvcWebSite.atomes.photos.Views
{
    public class PhotoFileResult : ActionResult
    {

        public PhotoFileResult()
        {
        }

        public PhotoFileSize Size { get; set; }
        public IPhotoInfo Photo { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            //if (!String.IsNullOrEmpty(FileDownloadName)) {
            //    context.HttpContext.Response.AddHeader("content-disposition",
            //      "attachment; filename=" + this.FileDownloadName);
            //}

            //string filePath = context.HttpContext.Server.MapPath(this.VirtualPath);
            var response = context.HttpContext.Response;

            FileInfo fileInfo = null;
            if (Size != PhotoFileSize.Raw)
            {
                var thumbFile = PhotoFileProvider.GetResizedPhoto(Photo.MediaFilePath, Size);
                fileInfo = new FileInfo(thumbFile);
            }
            else
            {
                fileInfo = new FileInfo(Photo.MediaFilePath);
            }
            response.AddHeader("Content-Length", fileInfo.Length.ToString());
            response.ContentType = "image/jpeg";
            response.Cache.SetExpires(DateTime.Now.Add(TimeSpan.FromDays(30)));
            response.Cache.SetCacheability(HttpCacheability.Public);
            response.Cache.SetValidUntilExpires(false);
            response.WriteFile(fileInfo.FullName);
            response.Flush();
        }
    }
}
