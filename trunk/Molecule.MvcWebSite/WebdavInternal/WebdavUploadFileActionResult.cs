using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace Molecule.MvcWebSite.WebdavInternal
{
    public class WebdavUploadFileActionResult : ActionResult
    {
        string filePath;
        int statusCode;
        Dictionary<string, string> headers;

        public WebdavUploadFileActionResult(string filePath, int statusCode, Dictionary<string, string> headers)
        {
            this.filePath = filePath;
            this.statusCode = statusCode;
            this.headers = headers;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.WriteFile(filePath);
            context.HttpContext.Response.StatusCode = statusCode;
            if (headers != null)
            {
                foreach (var h in headers)
                {
                    context.HttpContext.Response.AppendHeader(h.Key, h.Value);
                }
            }
        }
    }
}
