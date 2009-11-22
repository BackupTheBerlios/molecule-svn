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
    public class PreferencesController : PreferencesPageControllerBase<Atome>
    {


        public ActionResult Index()
        {
            var authorizableUsers = (from user in Membership.GetAllUsers().Cast<MembershipUser>()
                                      where !Roles.IsUserInRole(user.UserName, Molecule.SQLiteProvidersHelper.AdminRoleName)
                                      select user.UserName);

            var res = new PreferencesData() {
                DeletableUsers = from user in Membership.GetAllUsers().Cast<MembershipUser>()
                                 where !Roles.IsUserInRole(user.UserName, Molecule.SQLiteProvidersHelper.AdminRoleName)
                                 select new DeletableUserData() { Name = user.UserName, LastLoginDate = user.LastLoginDate },
                AuthorizableUsers = authorizableUsers.Concat(Resources.Common.Anonymous),
                Authorizations = from atome in AtomeService.GetAtomes()
                                 where !atome.AdminOnly
                                 select new AtomeUserAuthorizationsData(atome.Id,
                                     authorizableUsers.Concat(AtomeUserAuthorizations.AnonymousUser)),
                Title = AdminService.MoleculeTitle
            };
            return View(res);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult CreateUser()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateUser(string username, string password)
        {
            AdminService.CreateUser(username, password);
            return RedirectToAction<PreferencesController>(c => c.Index());
        }

        public ActionResult DeleteUser(string id)
        {
            AdminService.DeleteUser(id);
            return RedirectToAction<PreferencesController>(c => c.Index());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Save(string[] authorizations, string title)
        {
            //update autorizations
            foreach (var aua in AdminService.AtomeUserAuthorizations)
                foreach (var auth in aua.Authorizations) {
                    bool authorized = authorizations.Contains(AtomeUserAuthorizationData.GetValue(auth.Atome, auth.User));
                    AdminService.AtomeUserAuthorizations.Set(auth.Atome, auth.User, authorized);
                }

            AdminService.SaveAtomeUserAuthorizations();

            AdminService.MoleculeTitle = title;

            return RedirectToAction<PreferencesController>(c => c.Index());
        }
    }
}
