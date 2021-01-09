using System;
using System.Collections;
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

        public void Add(TKey key, TValue value)
        {
            _root = Add(_root, key, value);
        }
        
        private static Node Add(Node? node, TKey key, TValue value)
        {
            if (node is null) return new Node(key, value);
            switch (node.Key.CompareTo(key))
            {
                case < 0:
                    node.Right = Add(node.Right, key, value);
                    return node;
                case > 0:
                    node.Left = Add(node.Left, key, value);
                    return node;
                case 0:
                    node.Value = value;
                    return node;
            }
        }

        public bool TryGet(TKey key, out TValue? value)
        {
            return TryGet(_root, key, out value);
        }

        public void Delete(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool Contains(TKey key)
        {
            return TryGet(key, out _);
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
            return OrderedKeyValuePairs().Select(pair => pair.Key);
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
            return Keys().OrderBy(_ => 1);
        }

        public TKey Min()
        {
            throw new NotImplementedException();
        }

        public TKey Max()
        {
            throw new NotImplementedException();
        }

        private static bool TryGet(Node? node, TKey key, out TValue? value)
        {
            while (true)
            {
                if (node is null)
                {
                    value = default;
                    return false;
                }

                switch (node.Key.CompareTo(key))
                {
                    case < 0:
                        node = node.Right;
                        continue;
                    case > 0:
                        node = node.Left;
                        continue;
                    case 0:
                        value = node.Value;
                        return true;
                }
            }
        }

        // TODO: Add Left and/or Right nodes to constructor if they can be initialized at the record creation moment
        private record Node (TKey Key, TValue Value)
        {
            public Node? Left { get; set; }
            public Node? Right { get; set; }
            public TValue Value { get; set; } = Value;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return OrderedKeyValuePairs().GetEnumerator();
        }

        private Queue<KeyValuePair<TKey, TValue>> OrderedKeyValuePairs()
        {
            var pairsOrderedByKey = new Queue<KeyValuePair<TKey, TValue>>();
            TraverseInorder(_root, pairsOrderedByKey);
            return pairsOrderedByKey;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static void TraverseInorder(Node? node, Queue<KeyValuePair<TKey,TValue>> queue)
        {
            if (node is null) return;
            TraverseInorder(node.Left, queue);
            queue.Enqueue(KeyValuePair.Create(node.Key, node.Value));
            // ReSharper disable once TailRecursiveCall
            // Let's stick to symmetric recursive inorder traversal for now
            TraverseInorder(node.Right, queue);
        }
    }
}