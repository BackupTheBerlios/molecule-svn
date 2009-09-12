using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Molecule.MvcWebSite.Controllers;
using Molecule.MvcWebSite.atomes.admin.Data;
using System.Web.Mvc;
using Molecule.WebSite.Services;
using System.Web.Security;
using Molecule.Collections;

namespace Molecule.MvcWebSite.atomes.admin.Controllers
{
    public class PreferencesController : PreferencesPageControllerBase
    {
        public ActionResult Index()
        {
            var authorizableUsers = (from user in Membership.GetAllUsers().Cast<MembershipUser>()
                                      where !Roles.IsUserInRole(user.UserName, Molecule.SQLiteProvidersHelper.AdminRoleName)
                                      select user.UserName).Concat(Resources.Common.Anonymous);

            var res = new PreferencesData() {
                DeletableUsers = from user in Membership.GetAllUsers().Cast<MembershipUser>()
                                 where !Roles.IsUserInRole(user.UserName, Molecule.SQLiteProvidersHelper.AdminRoleName)
                                 select new DeletableUserData() { Name = user.UserName, LastLoginDate = user.LastLoginDate },
                AuthorizableUsers = authorizableUsers,
                Authorizations = from atome in AtomeService.GetAtomes()
                                 where !atome.AdminOnly
                                 select new AtomeUserAuthorizationsData(atome.Name,authorizableUsers)
            };
            return View(res);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Create()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string username, string password)
        {
            Membership.CreateUser(username, password);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            Membership.DeleteUser(id);
            return RedirectToAction("Index");
        }
    }
}
