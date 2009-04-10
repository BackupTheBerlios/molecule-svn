using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Molecule
{
    public static class ArrayExtensions
    {
        public static IEnumerable<T> AsEnumerable<T>(this Array array)
        {
            for (int i = 0; i < array.Length; i++ )
                yield return (T)array.GetValue(i);
        }
    }
}
