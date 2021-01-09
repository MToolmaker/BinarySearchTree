using System;
using System.Linq;
using JetBrains.Annotations;

namespace BinarySearchTree.SymbolTables
{
    [PublicAPI]
    public interface IOrderedSymbolTable<TKey, TValue> : ISymbolTable<TKey, TValue>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        public bool TryGetFloor(out TKey min);

        public bool TryGetCeiling(out TKey min);

        public int Rank(TKey key);

        public IOrderedEnumerable<TKey> OrderedKeys();
        
        public bool TryGetMin(out TKey min);
        
        public bool TryGetMax(out TKey min);
    }
}