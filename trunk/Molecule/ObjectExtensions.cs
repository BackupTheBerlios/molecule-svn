using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Molecule
{
    public static class ObjectExtensions
    {
        public static R NotNull<T,R>(this T obj, Func<T, R> func)
        {
            if (obj == null)
                return default(R);
            return func(obj);
        }
    }
}
