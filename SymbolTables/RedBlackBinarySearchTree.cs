using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

namespace BinarySearchTree.SymbolTables
{
    [PublicAPI]
    public class RedBlackBinarySearchTree<TKey, TValue> : IOrderedSymbolTable<TKey, TValue>
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

        public bool TryGetCeiling(TKey key, out TKey? ceiling)
        {
            return TryGetCeiling(_root, key, out ceiling);
        }

        public int Rank(TKey key)
        {
            return Rank(_root, key);
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

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return OrderedKeyValuePairs().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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
                        floor = minInRightSubtree.CompareTo(nodeKey) < 0 || minInRightSubtree.CompareTo(key) > 0
                            ? nodeKey
                            : minInRightSubtree;
                        return true;
                    case 0:
                        floor = key;
                        return true;
                }
            }
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

        private static Node GetMin(Node node)
        {
            while (node.Left is { } left) node = left;
            return node;
        }

        private static Node GetMax(Node node)
        {
            while (node.Right is { } right) node = right;
            return node;
        }

        private Node Add(Node? node, TKey key, TValue value)
        {
            if (node is null) return new Node(key, value, _root is not null? Color.Red : Color.Black);
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

            if (IsRed(node.Right) && !IsRed(node.Left)) node = RotateLeft(node);
            if (IsRed(node.Left) && IsRed(node.Left?.Left)) node = RotateRight(node);
            if (IsRed(node.Left) && IsRed(node.Right)) FlipColors(node);
            UpdateSize(node);
            return node;
        }

        private static void UpdateSize(Node node)
        {
            node.SubtreeSize = 1 + Size(node.Left) + Size(node.Right);
        }

        private static Node RotateLeft(Node node)
        {
            var right = node.Right;
            Debug.Assert(right is not null && IsRed(right));
            Node tmp = right;
            node.Right = node.Left;
            tmp.Left = node;
            tmp.Color = node.Color;
            node.Color = Color.Red;
            UpdateSize(tmp);
            UpdateSize(node);
            return tmp;
        }

        private static Node RotateRight(Node node)
        {
            var left = node.Left;
            Debug.Assert(left is not null && IsRed(left));
            Node tmp = left;
            node.Left = node.Right;
            tmp.Right = node;
            tmp.Color = node.Color;
            node.Color = Color.Red;
            UpdateSize(tmp);
            UpdateSize(node);
            return tmp;
        }

        private static void FlipColors(Node node)
        {
            var left = node.Left;
            Debug.Assert(left is not null && IsRed(left));
            var right = node.Right;
            Debug.Assert(right is not null && IsRed(right));
            node.Color = Color.Red;
            left.Color = Color.Black;
            right.Color = Color.Black;
        }

        private static bool IsRed(Node? node)
        {
            return node?.Color == Color.Red;
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

        private record Node (TKey Key, TValue Value, Color Color)
        {
            public Node? Left { get; set; }
            public Node? Right { get; set; }
            public TValue Value { get; set; } = Value;
            public Color Color { get; set; } = Color;
            public int SubtreeSize { get; set; } = 1;
        }

        private enum Color
        {
            Red,
            Black
        }
    }
}