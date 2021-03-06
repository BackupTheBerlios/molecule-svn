using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Molecule.MvcWebSite.WebdavInternal.HttpMethods;

namespace Molecule.MvcWebSite.Controllers
{
    public class WebdavController : Controller
    {
        //
        // GET: /WebDAV/
       
        public ActionResult Index()
        {
            if (this.Request.RequestType.Equals(OptionMethod.Name))
            {
                OptionMethod optionMethod = new OptionMethod();
                return optionMethod.HandleRequest(this);
            }
            else if (this.Request.RequestType.Equals(PropFindMethod.Name))
            {
                PropFindMethod propFindMethod = new PropFindMethod();
                return propFindMethod.HandleRequest(this);
            }
            else if (this.Request.RequestType.Equals(GetMethod.Name))
            {
                GetMethod getMethod = new GetMethod();
                return getMethod.HandleRequest(this);
            }
      
            return new EmptyResult();
        }

    }
}
