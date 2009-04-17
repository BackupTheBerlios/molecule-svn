using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace Molecule.WebSite.setup
{
    public partial class Default : System.Web.UI.Page
    {
        const string loopback = "127.0.0.1";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.UserHostAddress != loopback || Membership.GetAllUsers().Count > 0)
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
