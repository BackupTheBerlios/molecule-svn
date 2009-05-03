
using System;
using System.IO;
using System.Collections.Generic;
using WebPhoto.Services;
using WebPhoto.Providers;
using System.Web;
using System.Web.UI;
using System.Linq;
using ICSharpCode.SharpZipLib.Zip;

namespace Molecule.WebSite.atomes.photo
{
	
	
	public partial class Download : System.Web.UI.Page
	{
		
		private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(Download));
		protected void Page_Load(object sender, EventArgs e)
		{
			string tagId = Request.QueryString["id"];
			int  startIndex = Int32.Parse( Request.QueryString["start"]);
			int  numberOfElements = Int32.Parse( Request.QueryString["number"]);
			if(!String.IsNullOrEmpty(tagId))
				SendPhotos(tagId, startIndex, numberOfElements);
		}
		
		private void SendPhotos(string tagId, int startIndex, int numberOfElements)
		{
			string fileName = Path.GetTempFileName();
			string tagName = PhotoLibrary.GetTag(tagId).Name;
			List<IPhotoInfo> p =  PhotoLibrary.GetPhotosByTag(tagId).ToList<IPhotoInfo>();
			List<IPhotoInfo> photos = new List<IPhotoInfo>();
			for (int i = startIndex ; (i < p.Count) && (i < startIndex + numberOfElements ) ; i++)
			{
				photos.Add(p[i]);
			}
			
			if (log.IsDebugEnabled)
            {
                log.Debug("Starting creation of zip file : " + fileName);
            }				
			
            using (ZipOutputStream zipOutputStream = new ZipOutputStream(new FileStream(fileName, FileMode.OpenOrCreate)))
            {
                zipOutputStream.SetLevel(0);
                foreach (IPhotoInfo photo in photos)
                {
                    FileInfo fileInfo = new FileInfo(photo.MediaFilePath);
                    ZipEntry entry = new ZipEntry(tagName + @"\" + photo.Id+".jpg");
                    zipOutputStream.PutNextEntry(entry);
                    FileStream fs = File.OpenRead(photo.MediaFilePath);

                    byte[] buff = new byte[1024];
                    int n = 0;
                    while ((n = fs.Read(buff, 0, buff.Length)) > 0)
                    {
                        zipOutputStream.Write(buff, 0, n);

                    }
                    fs.Close();
                }
                zipOutputStream.Finish();
            }			

			System.IO.FileInfo file = new System.IO.FileInfo(fileName);
			Response.Clear();
			Response.AddHeader("Content-Disposition", "attachment; filename=" + "Photos.zip");
			Response.AddHeader("Content-Length", file.Length.ToString());
			Response.ContentType = "application/octet-stream";
			Response.WriteFile(file.FullName);
			Response.End();
			File.Delete(fileName);
			
            if (log.IsDebugEnabled)
            {
                log.Debug("Zip file created : " + fileName);
            }
		}
	
		public static string GetUrlFor(string tagId, int startIndex, int numberOfElements)
		{
			return String.Format("Download.aspx?id={0}&start={1}&number={2}", tagId, startIndex, numberOfElements);
		}
		
	}
	

	
}
