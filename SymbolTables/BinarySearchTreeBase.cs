using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace BinarySearchTree.SymbolTables
{
    [PublicAPI]
    public abstract class BinarySearchTreeBase<TNode, TKey, TValue> : IOrderedSymbolTable<TKey, TValue>
        where TNode : INode<TNode, TKey, TValue>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        protected TNode? Root;

        public abstract void Add(TKey key, TValue value);

        public abstract bool TryDelete(TKey key);

        public bool TryGet(TKey key, out TValue? value)
        {
            return TryGet(Root, key, out value);
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
            return Size(Root);
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
            return TryGetFloor(Root, key, out floor);
        }

        public bool TryGetCeiling(TKey key, out TKey? ceiling)
        {
            return TryGetCeiling(Root, key, out ceiling);
        }

        public int Rank(TKey key)
        {
            return Rank(Root, key);
        }

        public IOrderedEnumerable<TKey> OrderedKeys()
        {
            return Keys().OrderBy(_ => 1);
        }

        public bool TryGetMin(out TKey? min)
        {
            var root = Root;
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
            var root = Root;
            if (root is null)
            {
                max = default;
                return false;
            }

            max = GetMax(root).Key;
            return true;
        }

        public int RangeCount(TKey lo, TKey hi)
        {
            return Contains(hi) ? Rank(hi) - Rank(lo) + 1 : Rank(hi) - Rank(lo);
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> Range(TKey lo, TKey hi)
        {
            var pairsOrderedByKeyInRange = new Queue<KeyValuePair<TKey, TValue>>();
            TraverseInorderRange(Root, pairsOrderedByKeyInRange, lo, hi);
            return pairsOrderedByKeyInRange;
        }

        private static void TraverseInorderRange(TNode? node, Queue<KeyValuePair<TKey, TValue>> queue, TKey lo, TKey hi)
        {
            if (node is null) return;
            var key = node.Key;
            var isLowBoundLessThanKey = lo.CompareTo(key) <= 0;
            if (isLowBoundLessThanKey) TraverseInorderRange(node.Left, queue, lo, hi);
            var isKeyLessThanHighBound = key.CompareTo(hi) <= 0;
            if (isLowBoundLessThanKey && isKeyLessThanHighBound)
                queue.Enqueue(KeyValuePair.Create(node.Key, node.Value));
            if (isKeyLessThanHighBound) TraverseInorderRange(node.Right, queue, lo, hi);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return OrderedKeyValuePairs().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static bool TryGet(TNode? node, TKey key, out TValue? value)
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

        private static bool TryGetFloor(TNode? node, TKey key, out TKey? floor)
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

        private static bool TryGetCeiling(TNode? node, TKey? key, out TKey? ceiling)
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

        private int Rank(TNode? node, TKey? key)
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

        protected static TNode GetMin(TNode node)
        {
            while (node.Left is { } left) node = left;
            return node;
        }

        protected static TNode GetMax(TNode node)
        {
            while (node.Right is { } right) node = right;
            return node;
        }

        protected static int Size(TNode? node)
        {
            return node?.SubtreeSize ?? 0;
        }

        private Queue<KeyValuePair<TKey, TValue>> OrderedKeyValuePairs()
        {
            var pairsOrderedByKey = new Queue<KeyValuePair<TKey, TValue>>();
            TraverseInorder(Root, pairsOrderedByKey);
            return pairsOrderedByKey;
        }

        private static void TraverseInorder(TNode? node, Queue<KeyValuePair<TKey, TValue>> queue)
        {
            while (true)
            {
                if (node is null) return;
                TraverseInorder(node.Left, queue);
                queue.Enqueue(KeyValuePair.Create(node.Key, node.Value));
                node = node.Right;
            }
        }
    }

    public interface INode<out TNode, out TKey, out TValue>
    {
        public TNode? Left { get; }
        public TNode? Right { get; }
        public TKey Key { get; }
        public TValue Value { get; }
        public int SubtreeSize { get; }
    }
}