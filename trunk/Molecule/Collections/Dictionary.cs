using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Molecule.Collections
{
    public class Dictionary<TKey, TValue> :
        System.Collections.Generic.Dictionary<TKey, TValue>
        , IKeyedEnumerable<TKey, TValue>
    {
        #region IKeyedEnumerable<TKey,TValue> Members

        IEnumerable<TKey> IKeyedEnumerable<TKey, TValue>.Keys
        {
            get { return base.Keys; }
        }

        IEnumerable<TValue> IKeyedEnumerable<TKey, TValue>.Values
        {
            get { return base.Values; }
        }

        #endregion
    }
}
