using System;
using System.Linq;
using JetBrains.Annotations;

namespace BinarySearchTree.SymbolTables
{
    [PublicAPI]
    public interface IOrderedSymbolTable<TKey, TValue> : ISymbolTable<TKey, TValue>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        public TKey Floor(TKey key);

        public TKey Ceiling(TKey key);

        public int Rank(TKey key);

        public IOrderedEnumerable<TKey> OrderedKeys();
        
        public TKey Min();
        
        public TKey Max();
    }
}