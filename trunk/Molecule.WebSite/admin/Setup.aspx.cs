using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace Molecule.WebSite.admin
{
    public partial class Setup : System.Web.UI.Page
    {
        public const string loopback = "127.0.0.1";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Services.AdminService.IsSetupAuthorized)
            {
                Context.Response.Redirect("~/");
            }

            this.createUserWizard.UserName = Environment.UserName;

        }

        protected void createUserWizard_CreatedUser(object sender, EventArgs e)
        {
            Roles.AddUserToRole(this.createUserWizard.UserName, Molecule.SQLiteProvidersHelper.AdminRoleName);
        }
    }
}
