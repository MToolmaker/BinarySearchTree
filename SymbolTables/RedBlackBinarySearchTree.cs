using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace BinarySearchTree.SymbolTables
{
    [PublicAPI]
    public class RedBlackBinarySearchTree<TKey, TValue> :
        BinarySearchTreeBase<RedBlackBinarySearchTree<TKey, TValue>.Node, TKey, TValue>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        public enum Color
        {
            Red,
            Black
        }

        public override void Add(TKey key, TValue value)
        {
            Root = Add(Root, key, value);
        }

        public override bool TryDelete(TKey key)
        {
            throw new NotImplementedException("Delete operation isn't supported for now");
        }

        private static Node? DeleteMin(Node node)
        {
            if (node.Left is null) return node.Right;
            node.Left = DeleteMin(node);
            node.SubtreeSize = 1 + Size(node.Left) + Size(node.Right);
            return node;
        }

        private Node Add(Node? node, TKey key, TValue value)
        {
            if (node is null) return new Node(key, value, Root is not null ? Color.Red : Color.Black);
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

        public record Node (TKey Key, TValue Value, Color Color) : INode<Node, TKey, TValue>
        {
            public Color Color { get; set; } = Color;
            public Node? Left { get; set; }
            public Node? Right { get; set; }
            public TValue Value { get; set; } = Value;
            public int SubtreeSize { get; set; } = 1;
        }
    }
}