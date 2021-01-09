using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace BinarySearchTree.SymbolTables
{
    [PublicAPI]
    public interface ISymbolTable<TKey, TValue> 
        where TKey : IEquatable<TKey>
    {
        void Add(TKey key, [NotNull] TValue value);
        
        bool TryGet(TKey key, out TValue? value);

        void Delete(TKey key);

        bool Contains(TKey key);

        bool IsEmpty();

        int Size();
        
        [NotNull]
        IEnumerable<TKey> Keys();
        
        [NotNull, ItemNotNull]
        IEnumerable<TValue> Values();
    }
}