using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace BST.IntervalTree
{
    [PublicAPI]
    public class IntervalTree
    {
        private Node? _root;

        public (double, double)? FindIntersection(double a, double b)
        {
            var node = _root;
            while (node is not null)
            {
                var currentInterval = (node.Key, node.Value);
                if (Intersects((a, b), currentInterval)) return currentInterval;
                var left = node.Left;
                if (left is null || left.MaxRightEndpoint < a) node = node.Right;
                else node = left;
            }

            return null;
        }

        public HashSet<(double, double)> FindIntersections(double a, double b)
        {
            var intersections = new HashSet<(double, double)>();
            FindIntersections(_root, a, b, intersections);
            return intersections;
        }

        private static void FindIntersections(Node? node, double a, double b, ISet<(double, double)> intersections)
        {
            while (true)
            {
                if (node is null) return;
                var currentInterval = (node.Key, node.Value);
                if (Intersects((a, b), currentInterval)) intersections.Add(currentInterval);
                var left = node.Left;
                if (left is not null && left.MaxRightEndpoint > a) FindIntersections(left, a, b, intersections);
                node = node.Right;
            }
        }

        public void Add((double, double) interval)
        {
            _root = Add(_root, interval.Item1, interval.Item2);
        }

        private static bool Intersects((double, double) firstInterval, (double, double) secondInterval)
        {
            var (firstIntervalStart, firstIntervalEnd) = firstInterval;
            var (secondIntervalStart, secondIntervalEnd) = secondInterval;
            return !(firstIntervalEnd < secondIntervalStart || secondIntervalEnd < firstIntervalStart);
        }


        private static Node? DeleteMin(Node node)
        {
            if (node.Left is null) return node.Right;
            node.Left = DeleteMin(node);
            node.SubtreeSize = 1 + Size(node.Left) + Size(node.Right);
            return node;
        }

        private Node Add(Node? node, double key, double value)
        {
            if (node is null) return new Node(key, value, _root is not null ? Color.Red : Color.Black);
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
            UpdateMaxRightEndpoint(node);
            return node;
        }

        private static void UpdateSize(Node node)
        {
            node.SubtreeSize = 1 + Size(node.Left) + Size(node.Right);
        }

        private static int Size(Node? node) => node?.SubtreeSize ?? 0;

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
            UpdateMaxRightEndpoint(node);
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
            UpdateMaxRightEndpoint(tmp);
            UpdateMaxRightEndpoint(node);
            return tmp;
        }

        private static void UpdateMaxRightEndpoint(Node node)
        {
            var left = node.Left;
            var right = node.Right;
            node.MaxRightEndpoint = (left, right) switch
            {
                (null, null) => node.Value,
                (null, not null) => Math.Max(node.Value, right.MaxRightEndpoint),
                (not null, null) => Math.Max(node.Value, left.MaxRightEndpoint),
                (not null, not null) => Math.Max(node.Value, Math.Max(left.MaxRightEndpoint, right.MaxRightEndpoint))
            };
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

        private static bool IsRed(Node? node) => node?.Color == Color.Red;

        private enum Color
        {
            Red,
            Black
        }

        private record Node (double Key, double Value, Color Color)
        {
            public Color Color { get; set; } = Color;
            public Node? Left { get; set; }
            public Node? Right { get; set; }
            public double Value { get; set; } = Value;
            public int SubtreeSize { get; set; } = 1;
            public double MaxRightEndpoint { get; set; }
        }
    }
}