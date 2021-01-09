using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace BinarySearchTree.SymbolTables
{
    [PublicAPI]
    public class SimpleBinarySearchTree<TKey, TValue> : IUnorderedSymbolTable<TKey, TValue>
        where TKey : struct, IEquatable<TKey>
    {
        public void Put(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        public bool TryGet(TKey key, out TValue value)
        {
            throw new NotImplementedException();
        }

        public void Delete(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool Contains(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool IsEmpty()
        {
            throw new NotImplementedException();
        }

        public int Size()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TKey> Keys()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TValue> Values()
        {
            throw new NotImplementedException();
        }
    }
}