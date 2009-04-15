using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Molecule.Atome;
using System.Reflection;

namespace Molecule.WebSite
{
    public partial class ProviderSelector : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ProviderList.DataSource = Providers;
                ProviderList.DataBind();
                ProviderList.SelectedValue = CurrentProvider;
            }
        }

        protected void ProviderList_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentProvider = ProviderList.SelectedValue;
        }

        protected PropertyInfo ProvidersProperty
        {
            get { return AtomeProviderType.GetProperty("Providers",
                BindingFlags.FlattenHierarchy | BindingFlags.Static | BindingFlags.Public); }
        }

        protected IEnumerable<ProviderInfo> Providers
        {
            get { return (IEnumerable<ProviderInfo>)ProvidersProperty.GetValue(null, null); }
        }

        protected PropertyInfo CurrentProviderProperty
        {
            get { return AtomeProviderType.GetProperty("CurrentProvider",
                BindingFlags.FlattenHierarchy | BindingFlags.Static | BindingFlags.Public); }
        }
        protected string CurrentProvider
        {
            get
            {
                return (string)CurrentProviderProperty.GetValue(null, null);
            }
            set
            {
                CurrentProviderProperty.SetValue(null, value, null);
            }
        }

        Type atomeProviderType;
        protected Type AtomeProviderType
        {
            get
            {
                if (atomeProviderType == null)
                {
                    if (String.IsNullOrEmpty(AtomeProviderTypeName))
                        throw new ApplicationException("AtomeProviderTypeName is null or empty.");
                    atomeProviderType = Type.GetType(AtomeProviderTypeName);
                    if (atomeProviderType.BaseType.Name != "AtomeProviderBase`2")
                        throw new ApplicationException("AtomeProviderTypeName base type must be AtomeProviderBase`2");
                }
                return atomeProviderType;
            }
        }

        public string AtomeProviderTypeName { get; set; }
    }
}