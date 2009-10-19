﻿
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Molecule.Webdav;
using Molecule.Web;

namespace Molecule.MvcWebSite.WebdavInternal.HttpMethods
{


    public class GetMethod
    {

        public GetMethod()
        {
        }

        public void HandleRequest(Controller context)
        {
            string[] pathElements = context.Request.Path.Split('/');
            if (pathElements.Length > 1)
            {
                IVirtualWebdavFolder virtualWebdavFolder = Singleton<VirtualWebdavFolderService>.Instance.GetVirtualWebdavFolder(pathElements[2]);
                if (virtualWebdavFolder != null)
                {
                    // TODO : clean up the parsing of the path
                    string path = context.Request.Path.Substring((pathElements[0] + "/" + pathElements[1] + "/"+pathElements[2]+"/").Length);
                    string filePath = virtualWebdavFolder.GetFile(path);
                    if (!String.IsNullOrEmpty(filePath))
                    {
                        context.Response.WriteFile(filePath);
                        context.Response.StatusCode = 200;
                        // TODO: add a real etag support
                        context.Response.AppendHeader("Etag", "b0c5faef67f106ef634ad2a82e838b95");
                        context.Response.End();
                    }
                }

            }
        }

        public static string Name
        {
            get
            {
                return "GET";
            }
        }
    }
}