using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace BST.SymbolTables
{
    [PublicAPI]
    public interface ISymbolTable<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
        where TKey : IEquatable<TKey>
    {
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        void Add(TKey key, [NotNull] TValue value);

        bool TryGet(TKey key, out TValue? value);

        bool TryDelete(TKey key);

        bool Contains(TKey key);

        bool IsEmpty();

        int Size();

        [NotNull]
        IEnumerable<TKey> Keys();

        [NotNull]
        [ItemNotNull]
        IEnumerable<TValue> Values();
    }
}