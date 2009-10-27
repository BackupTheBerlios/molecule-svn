
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Molecule.Collections;

namespace Molecule.MvcWebSite.WebdavInternal.HttpMethods
{
	
	
	public class OptionMethod
	{
		
		public OptionMethod()
		{
		}
		
		public ActionResult HandleRequest(Controller context)
		{
            // Send back a hard coded response
            // MS-Author header is needed to work with the vista's webdav browser 
            WebdavResult actionResult = new WebdavResult(String.Empty, 200, new Dictionary<string, string> { 
                                                                                            { "DAV", "1,2" },
                                                                                            { "MS-Author-Via", "DAV" },
                                                                                            { "Allow", "GET, HEAD, POST, PUT, DELETE, OPTIONS, PROPFIND, MKCOL, LOCK, UNLOCK"}
                                                                                            });
            return actionResult;
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
