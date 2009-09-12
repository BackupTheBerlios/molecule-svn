using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Molecule.Collections
{
    public static class CollectionExtensions
    {
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> list, T item)
        {
            foreach (var i in list)
                yield return i;
            yield return item;
        }
    }
}
