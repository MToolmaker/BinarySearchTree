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

        public bool TryGet(TKey key, out TValue? value)
        {
            return TryGet(_root, key, out value);
        }

        public bool TryDelete(TKey key)
        {
            _root = Delete(_root, key, out var deleted);
            return deleted;
        }

        private static Node? Delete(Node? node, TKey key, out bool deleted)
        {
            if (node is null)
            {
                deleted = false;
                return null;
            }

            var compareResult = node.Key.CompareTo(key);
            var leftChild = node.Left;
            var rightChild = node.Right;
            switch (compareResult)
            {
                case > 0:
                    node.Left = Delete(leftChild, key, out deleted);
                    break;
                case < 0:
                    node.Right = Delete(rightChild, key, out deleted);
                    break;
                case 0:
                    deleted = true;
                    if (leftChild is null) return rightChild;
                    if (rightChild is null) return leftChild;
                    node = GetMin(rightChild);
                    node.Right = DeleteMin(rightChild);
                    node.Left = leftChild;
                    break;
            }

            node.SubtreeSize = 1 + Size(leftChild) + Size(rightChild);
            return node;
        }

        private static Node? DeleteMin(Node node)
        {
            if (node.Left is null) return node.Right;
            node.Left = DeleteMin(node);
            node.SubtreeSize = 1 + Size(node.Left) + Size(node.Right);
            return node;
        }

        public bool Contains(TKey key)
        {
            return TryGet(key, out _);
        }

        public bool IsEmpty()
        {
            return Size() == 0;
        }

        public int Size()
        {
            return Size(_root);
        }

        public IEnumerable<TKey> Keys()
        {
            return OrderedKeyValuePairs().Select(pair => pair.Key);
        }

        public IEnumerable<TValue> Values()
        {
            return OrderedKeyValuePairs().Select(pair => pair.Value);
        }

        public bool TryGetFloor(TKey key, out TKey? floor)
        {
            return TryGetFloor(_root, key, out floor);
        }

        private static bool TryGetFloor(Node? node, TKey key, out TKey? floor)
        {
            while (true)
            {
                if (node is null)
                {
                    floor = default;
                    return false;
                }
                
                var nodeKey = node.Key;
                var compareResult = nodeKey.CompareTo(key);
                switch (compareResult)
                {
                    case > 0:
                        node = node.Left;
                        continue;
                    case < 0:
                        var rightChild = node.Right;
                        if (rightChild is null)
                        {
                            floor = nodeKey;
                            return true;
                        }

                        var minInRightSubtree = GetMin(rightChild).Key;
                        floor = minInRightSubtree.CompareTo(nodeKey) > 0 ? nodeKey : minInRightSubtree;
                        return true;
                    case 0:
                        floor = key;
                        return true;
                }
            }
        }

        public bool TryGetCeiling(TKey key, out TKey? ceiling)
        {
            return TryGetCeiling(_root, key, out ceiling);
        }
        
        private static bool TryGetCeiling(Node? node, TKey? key, out TKey? ceiling)
        {
            while (true)
            {
                if (node is null)
                {
                    ceiling = default;
                    return false;
                }
                
                var nodeKey = node.Key;
                var compareResult = nodeKey.CompareTo(key);
                switch (compareResult)
                {
                    case < 0:
                        node = node.Right;
                        continue;
                    case > 0:
                        var leftChild = node.Left;
                        if (leftChild is null)
                        {
                            ceiling = nodeKey;
                            return true;
                        }

                        var maxInLeftSubtree = GetMax(leftChild).Key;
                        ceiling = maxInLeftSubtree.CompareTo(nodeKey) > 0 ? maxInLeftSubtree : nodeKey;
                        return true;
                    case 0:
                        ceiling = key;
                        return true;
                }
            }

        }

        public int Rank(TKey key)
        {
            return Rank(_root, key);
        }

        private int Rank(Node? node, TKey? key)
        {
            while (true)
            {
                if (node is null) return 0;
                switch (node.Key.CompareTo(key))
                {
                    case > 0:
                        node = node.Left;
                        continue;
                    case < 0:
                        return 1 + Size(node.Left) + Rank(node.Right, key);
                    case 0:
                        return Size(node.Left);
                }
            }
        }

        public IOrderedEnumerable<TKey> OrderedKeys()
        {
            return Keys().OrderBy(_ => 1);
        }

        public bool TryGetMin(out TKey? min)
        {
            var root = _root;
            if (root is null)
            {
                min = default;
                return false;
            }

            min = GetMin(root).Key;
            return true;
        }

        private static Node GetMin(Node node)
        {
            while (node.Left is {} left) node = left;
            return node;
        }
        
        public bool TryGetMax(out TKey? max)
        {
            var root = _root;
            if (root is null)
            {
                max = default;
                return false;
            }

            max = GetMax(root).Key;
            return true;
        }

        private static Node GetMax(Node node)
        {
            while (node.Right is { } right) node = right;
            return node;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return OrderedKeyValuePairs().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static Node Add(Node? node, TKey key, TValue value)
        {
            if (node is null) return new Node(key, value);
            switch (node.Key.CompareTo(key))
            {
                case < 0:
                    node.Right = Add(node.Right, key, value);
                    break;
                case > 0:
                    node.Left = Add(node.Left, key, value);
                    break;
                case 0:
                    node.Value = value;
                    break;
            }

            node.SubtreeSize = 1 + Size(node.Left) + Size(node.Right);
            return node;
        }

        private static int Size(Node? node)
        {
            return node?.SubtreeSize ?? 0;
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

        private Queue<KeyValuePair<TKey, TValue>> OrderedKeyValuePairs()
        {
            var pairsOrderedByKey = new Queue<KeyValuePair<TKey, TValue>>();
            TraverseInorder(_root, pairsOrderedByKey);
            return pairsOrderedByKey;
        }

        private static void TraverseInorder(Node? node, Queue<KeyValuePair<TKey, TValue>> queue)
        {
            while (true)
            {
                if (node is null) return;
                TraverseInorder(node.Left, queue);
                queue.Enqueue(KeyValuePair.Create(node.Key, node.Value));
                node = node.Right;
            }
        }


        // TODO: Add Left and/or Right nodes to constructor if they can be initialized at the record creation moment
        private record Node (TKey Key, TValue Value)
        {
            public Node? Left { get; set; }
            public Node? Right { get; set; }
            public TValue Value { get; set; } = Value;
            public int SubtreeSize { get; set; } = 1;
        }
    }
}