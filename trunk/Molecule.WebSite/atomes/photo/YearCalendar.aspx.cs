using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace Molecule.WebSite.atomes.photo
{
    public partial class YearCalendar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected static string FormatMonth(int month)
        {
            return DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
        }
    }
}
