using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using Molecule.Runtime;

namespace Molecule.Atome
{
    public class ProviderInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Id { get { return Name; } }
    }
}
