
using System;
using System.IO;

using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Collections.Specialized;
using System.Xml;
using Molecule.WebSite.Services;
using Molecule.Webdav;
using Molecule.Web;


namespace Molecule.MvcWebSite.WebdavInternal.HttpMethods
{
	
	
	public class PropFindMethod
	{

        // Dictionary<string, IFileProvider> fileProviders;
        private string webDavRoot;

		public PropFindMethod()
		{
            // fileProviders = new Dictionary<string, IFileProvider>();
            // DirectoryProvider dp = new DirectoryProvider();
            // fileProviders.Add(dp.RootDirectoryName, dp);
            webDavRoot = "webdav/";
		}

        private string GenerateResponse(int depth, List<WebdavFileInfo> infoFilesListed)
        {
            XNamespace WebDavNameSpace = "DAV:";
            XNamespace ApacheNameSpance = "http://apache.org/dav/props/";

            List<WebdavFileInfo> filesInfoToSend = infoFilesListed;
            if (depth > 0)
            {
                filesInfoToSend = infoFilesListed.Concat<WebdavFileInfo>(from fp in Singleton<VirtualWebdavFolderService>.Instance.GetVirtualWebdavFolderRootsName()
                                                                              select new WebdavFileInfo { LastAccessTime = DateTime.Now, 
                                                                                  IsFile = false, 
                                                                                  Size = 4096, 
                                                                                  FileName = fp }).ToList<WebdavFileInfo>();
            }
            
            // add the root, don't knwo how to retrieve the size
            // so by default it will be 4096
            DirectoryInfo dirInfo = new DirectoryInfo("c:\\test");

            XDocument doc = new XDocument(
                            new XDeclaration("1.0", "UTF-8", "yes"),

                            new XElement(WebDavNameSpace + "multistatus", new XAttribute(XNamespace.Xmlns + "D", WebDavNameSpace),
                                        from fi in filesInfoToSend
                                        select new XElement(WebDavNameSpace + "response", new XAttribute(XNamespace.Xmlns + "default", ApacheNameSpance),
                                                new XElement(WebDavNameSpace + "href", "http://127.0.0.1:1110/webdav/" + fi.FileName.Replace('\\', '/')),
                                                new XElement(WebDavNameSpace + "propstat",
                                                    new XElement(WebDavNameSpace + "prop",
                                                        new XElement(WebDavNameSpace + "getlastmodified", fi.LastAccessTime.ToString("ddd, d MMM yyyy hh:mm:ss zzz", CultureInfo.CreateSpecificCulture("en-US"))),
                                                        new XElement(WebDavNameSpace + "getcontentlength", fi.Size),
                                                        new XElement(WebDavNameSpace + "resourcetype",
                                                             fi.IsFile ? (object)String.Empty : (object)new XElement(WebDavNameSpace + "collection", String.Empty))
                                                                ),
                                                        new XElement(WebDavNameSpace + "status", "HTTP/1.1 200 OK")
                                                            ),
                                                new XElement(WebDavNameSpace + "propstat", new XAttribute(XNamespace.Xmlns + "default", ApacheNameSpance),
                                                    new XElement(WebDavNameSpace + "prop",
                                                        new XElement(WebDavNameSpace + "checked-in", String.Empty),
                                                        new XElement(WebDavNameSpace + "checked-out", String.Empty),
                                                        new XElement(ApacheNameSpance + "executable", String.Empty)),
                                                    new XElement(WebDavNameSpace + "status", "HTTP/1.1 404 Not Found")
                                                )
                                          )
                            )
                    );
            infoFilesListed = null;
            doc.Save("c:\\template.xml");
            return doc.ToString();
        }

        private bool IsWebDavRoot(string path)
        {

            return false;
        }


		public void HandleRequest(Controller context)
		{
            string depthRaw = context.Request.Headers.Get("Depth");
            if (depthRaw == null)
            {
                return;
            }
            int depth;
            if (!Int32.TryParse(depthRaw, out depth))
            {
                return;
            }


            var atomes = AtomeService.GetAtomes();
        
            // look if we have to parse the request to parse the request to an IFileProvider
            // remove first character which is a /
            string filePath = context.Request.FilePath.Remove(0,1); 
            string[] filePathElements = filePath.Split('/');
            
            IVirtualWebdavFolder fileProvider = null;
            List<WebdavFileInfo> infoFilesEnumerable = null;
            bool isRoot = false;
            // loosy method to test if the client want to list the root
            if (filePathElements.Length > 1)
            {
                fileProvider = Singleton<VirtualWebdavFolderService>.Instance.GetVirtualWebdavFolder(filePathElements[1]);
                if (fileProvider != null)
                {
                    infoFilesEnumerable = fileProvider.List(filePath.Substring(filePathElements[0].Length + 1)).ToList<WebdavFileInfo>();
                }
            }

            if (filePathElements.Length <= 2)
            {
                // it's the root
                isRoot = true;
                // so we add the collection
                if (infoFilesEnumerable == null)
                {
                    infoFilesEnumerable = new List<WebdavFileInfo>();
                }
                infoFilesEnumerable.Insert(0, new WebdavFileInfo { LastAccessTime = DateTime.Now, FileName = String.Empty, Size = 4096 });
                infoFilesEnumerable.Insert(1, new WebdavFileInfo { LastAccessTime = DateTime.Now, FileName = "collection", Size = 4096 });
            }


            string res = this.GenerateResponse(depth, infoFilesEnumerable);
			using (StreamReader sr = new StreamReader(context.Request.InputStream))
			{
				Console.WriteLine("Lecture" +sr.ReadToEnd());
			}
			context.Response.Write(res);
			context.Response.StatusCode = 207;
			context.Response.ContentType = "text/xml";
			context.Response.End();
		}
		
		public static string Name
		{
			get
			{
				return "PROPFIND";
			}
		}	
		
	}
}
