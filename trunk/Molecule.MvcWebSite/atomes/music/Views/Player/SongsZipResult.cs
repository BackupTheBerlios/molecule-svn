using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMusic.Providers;
using WebMusic.IO;
using System.IO;

namespace Molecule.MvcWebSite.atomes.music.Views.Player
{
    public class SongsZipResult : ActionResult
    {
        string zipPath;
        public SongsZipResult(IEnumerable<ISong> songs)
            : base()
        {
            zipPath = Path.GetTempFileName();
            CompressionHelper.CompressFiles(songs, zipPath);
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            System.IO.FileInfo file = new System.IO.FileInfo(zipPath);
            response.Clear();
            response.AddHeader("Content-Disposition", "attachment; filename=" + "MusicSelection.zip");
            response.AddHeader("Content-Length", file.Length.ToString());
            response.ContentType = "application/octet-stream";
            response.WriteFile(file.FullName);
            response.End();
            File.Delete(zipPath);
        }
    }
}
