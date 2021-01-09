using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace BinarySearchTree.SymbolTables
{
    [PublicAPI]
    public class SimpleBinarySearchTree<TKey, TValue> : IOrderedSymbolTable<TKey, TValue>
        where TKey :  IComparable<TKey>, IEquatable<TKey>
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

        public TKey Floor(TKey key)
        {
            throw new NotImplementedException();
        }

        public TKey Ceiling(TKey key)
        {
            throw new NotImplementedException();
        }

        public int Rank(TKey key)
        {
            throw new NotImplementedException();
        }

        public IOrderedEnumerable<TKey> OrderedKeys()
        {
            throw new NotImplementedException();
        }

        public TKey Min()
        {
            throw new NotImplementedException();
        }

        public TKey Max()
        {
            throw new NotImplementedException();
        }

        private record Node (TKey Key, TValue Value, Node? Left, Node? Right);
    }
}