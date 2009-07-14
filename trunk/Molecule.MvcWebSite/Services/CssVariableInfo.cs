using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;

namespace Molecule.WebSite.Services
{
    [Serializable]
    public class CssVariableInfo : IComparable<CssVariableInfo>
    {
        public CssVariableInfo() { }
        public string Key { get; set; }
        public string Value { get; set; }

        static XmlSerializer ser = new XmlSerializer(typeof(List<CssVariableInfo>));

        public static IEnumerable<CssVariableInfo> Deserialize(string variables)
        {
            return (List<CssVariableInfo>)ser.Deserialize(new StringReader(variables));
        }

        public static string Serialize(IEnumerable<CssVariableInfo> variables)
        {
            var sb = new StringBuilder();
            ser.Serialize(new StringWriter(sb), new List<CssVariableInfo>(variables));
            return sb.ToString();
        }

        #region IComparable<CssVariableInfo> Members

        public int CompareTo(CssVariableInfo other)
        {
            return Key.CompareTo(other.Key);
        }

        #endregion
    }

    public class CssVariableInfoComparer : IEqualityComparer<CssVariableInfo>
    {

        #region IEqualityComparer<CssVariableInfo> Members

        public bool Equals(CssVariableInfo x, CssVariableInfo y)
        {
            return x.Key == y.Key;
        }

        public int GetHashCode(CssVariableInfo obj)
        {
            return obj.GetHashCode();
        }

        #endregion
    }
}
