using System;
using System.Linq;
using JetBrains.Annotations;

namespace BinarySearchTree.SymbolTables
{
    [PublicAPI]
    public interface IOrderedSymbolTable<TKey, TValue> : ISymbolTable<TKey, TValue>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        public bool TryGetFloor(out TKey? floor);

        public bool TryGetCeiling(out TKey? ceiling);

        public int Rank(TKey key);

        public IOrderedEnumerable<TKey> OrderedKeys();
        
        public bool TryGetMin(out TKey? min);
        
        public bool TryGetMax(out TKey? max);
    }
}