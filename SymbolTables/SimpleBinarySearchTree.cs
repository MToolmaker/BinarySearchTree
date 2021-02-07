using System;
using JetBrains.Annotations;

namespace BST.SymbolTables
{
    [PublicAPI]
    public class SimpleBinarySearchTree<TKey, TValue> :
        BinarySearchTreeBase<SimpleBinarySearchTree<TKey, TValue>.Node, TKey, TValue>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        public override void Add(TKey key, TValue value)
        {
            Root = Add(Root, key, value);
        }

        public override bool TryDelete(TKey key)
        {
            Root = Delete(Root, key, out var deleted);
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

        public record Node (TKey Key, TValue Value) : INode<Node, TKey, TValue>
        {
            public Node? Left { get; set; }
            public Node? Right { get; set; }
            public TValue Value { get; set; } = Value;
            public int SubtreeSize { get; set; } = 1;
        }
    }
}