using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace BinarySearchTree.SymbolTables
{
    [PublicAPI]
    public interface IUnorderedSymbolTable<TKey, TValue> 
        where TKey : struct, IEquatable<TKey>
    {
        void Put(TKey key, [NotNull] TValue value);
        
        bool TryGet(TKey key, [CanBeNull] out TValue value);

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