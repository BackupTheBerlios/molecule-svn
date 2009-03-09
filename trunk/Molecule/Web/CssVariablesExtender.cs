using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using Mono.Rocks;

namespace Molecule.Web
{
    /// <summary>
    /// Allow use of CSS Variables extensions
    /// http://disruptive-innovations.com/zoo/cssvariables/
    /// </summary>
    public class CssVariablesExtender
    {
        static Regex variablesRegex = new Regex(@"@variables\s*\{(?<decl>[^\}]*)\}", RegexOptions.Compiled);
        static Regex keyValueRegex = new Regex(@"\s*(?<key>[^:]+)\s*:\s*(?<value>[^;]+)\s*;", RegexOptions.Compiled);
        static Regex variableRefRegex = new Regex(@"var\((?<var>[^\)]+)\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static string ExpandVariables(string variablesCss)
        {
            return ExpandVariables(variablesCss, (pairs) => { });
        }

        public static string ExpandVariables(string variablesCss, IDictionary<string, string> overrides)
        {
            return ExpandVariables(variablesCss, (pairs) =>
                    overrides.ForEach(kvp => pairs[kvp.Key] = kvp.Value));
        }

        public static string ExpandVariables(string variablesCss, NameValueCollection overrides)
        {
            return ExpandVariables(variablesCss, delegate(IDictionary<string, string> pairs){
                for (int i = 0; i < overrides.Count; i++)
                    pairs[overrides.GetKey(i)] = overrides.Get(i);
            });
        }

        public static IDictionary<string, string> ExtractVariables(string variablesCss)
        {
            string outCss;
            return ExtractVariables(variablesCss, out outCss);
        }

        public static IDictionary<string, string> ExtractVariables(string variablesCss, out string standardCss)
        {
            var pairs = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            standardCss = variablesRegex.Replace(variablesCss, delegate(Match m){
                foreach (Match mkv in keyValueRegex.Matches(m.Groups[1].Value))
                    pairs[mkv.Groups["key"].Value] = mkv.Groups["value"].Value;
                return "";
            });
            return pairs;
        }

        public static string ExpandVariables(string variablesCss, Action<IDictionary<string,string>> overrideAction)
        {
            //TODO : cache
            //TODO : handle @import

            //search for variables definition & remove it from css
            string standardCss;
            var pairs = ExtractVariables(variablesCss, out standardCss);

            overrideAction(pairs);

            //search for variable reference & replace it by its definition.
            standardCss = variableRefRegex.Replace(standardCss, m => pairs[m.Groups["var"].Value]);
            return standardCss;
        }
    }
}
