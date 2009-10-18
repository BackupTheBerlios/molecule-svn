
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace Molecule.MvcWebSite.WebdavInternal.HttpMethods
{
	
	
	public class OptionMethod
	{
		
		public OptionMethod()
		{
		}
		
		public void HandleRequest(Controller context)
		{
            // Send back a hard coded response
            context.Response.AppendHeader("Allow", "GET, HEAD, POST, PUT, DELETE, OPTIONS, PROPFIND, MKCOL, LOCK, UNLOCK");
			context.Response.AppendHeader("DAV", "1,2");			
			context.Response.End();
		}
		
		public static string Name
		{
			get
			{
				return "OPTIONS";
			}
		}
	}
}
