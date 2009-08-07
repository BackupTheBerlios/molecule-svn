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
    public class PhotoFileResult : FilePathResult
    {

        public PhotoFileResult(IPhotoInfo photo, PhotoFileSize size)
            : base(getFile(photo, size), "image/jpeg")
        {
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.Cache.SetExpires(DateTime.Now.Add(TimeSpan.FromDays(30)));
            response.Cache.SetCacheability(HttpCacheability.Public);
            response.Cache.SetValidUntilExpires(false);
            base.ExecuteResult(context);
        }

        private static string getFile(IPhotoInfo photo, PhotoFileSize size)
        {
            return size != PhotoFileSize.Raw ? PhotoFileProvider.GetResizedPhoto(photo.MediaFilePath, size)
                                             : photo.MediaFilePath;
        }
    }
}
