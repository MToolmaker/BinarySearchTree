using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace BinarySearchTree.SymbolTables
{
    [PublicAPI]
    public class SimpleBinarySearchTree<TKey, TValue> : IOrderedSymbolTable<TKey, TValue>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        private Node? _root;

        public void Put(TKey key, TValue value)
        {
            _root = Put(_root, key, value);
        }
        
        private static Node Put(Node? node, TKey key, TValue value)
        {
            if (node is null) return new Node(key, value);
            switch (node.Key.CompareTo(key))
            {
                case < 0:
                    node.Right = Put(node.Right, key, value);
                    return node;
                case > 0:
                    node.Left = Put(node.Left, key, value);
                    return node;
                case 0:
                    node.Value = value;
                    return node;
            }
        }

        public bool TryGet(TKey key, out TValue? value)
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

        // TODO: Add Left and/or Right nodes to constructor if they can be initialized at the record creation moment
        private record Node (TKey Key, TValue Value)
        {
            public Node? Left { get; set; }
            public Node? Right { get; set; }
            public TValue Value { get; set; } = Value;
        }
    }
}