using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Molecule.Runtime
{
    [global::System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class IsUsablePluginAttribute : Attribute
    {
        // This is a positional argument
        public IsUsablePluginAttribute()
        {
            
        }
    }
}
