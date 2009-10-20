using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace Molecule.MvcWebSite.WebdavInternal
{
    public class WebdavActionResult : ActionResult
    {
        string content;
        int statusCode;
        Dictionary<string, string> headers;

        public WebdavActionResult( string content, int statusCode,   Dictionary<string, string> headers)
        {
            this.content = content;
            this.statusCode = statusCode;
            this.headers = headers;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Write(content);
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
