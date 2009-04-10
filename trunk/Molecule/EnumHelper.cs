using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Molecule
{
    public static class EnumHelper
    {
        public static IEnumerable<TEnum> GetValues<TEnum>()
        {
            return Enum.GetValues(typeof(TEnum)).AsEnumerable<TEnum>();
        }
    }
}
