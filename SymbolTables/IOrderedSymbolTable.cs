using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace BST.SymbolTables
{
    [PublicAPI]
    public interface IOrderedSymbolTable<TKey, TValue> : ISymbolTable<TKey, TValue>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        public bool TryGetFloor(TKey key, out TKey? floor);

        public bool TryGetCeiling(TKey key, out TKey? ceiling);

        public int Rank(TKey key);

        public IOrderedEnumerable<TKey> OrderedKeys();

        public bool TryGetMin(out TKey? min);

        public bool TryGetMax(out TKey? max);

        public int RangeCount(TKey lo, TKey hi);

        public IEnumerable<KeyValuePair<TKey, TValue>> Range(TKey lo, TKey hi);
    }
}