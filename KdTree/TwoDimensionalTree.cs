using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace BST.KdTree
{
    [PublicAPI]
    public class TwoDimensionalTree
    {
        private Node? _root;

        public void Add(double x, double y)
        {
            _root = Add(_root, x, y, PlanePartitioning.Vertical);
        }

        private static Node Add(Node? node, double x, double y, PlanePartitioning partitioning)
        {
            if (node is null) return new Node(x, y);
            switch (ChooseHalfPlane(node, x, y, partitioning))
            {
                case < 0:
                    node.Right = Add(node.Right, x, y, partitioning.Flip());
                    break;
                case > 0:
                    node.Left = Add(node.Left, x, y, partitioning.Flip());
                    break;
                case 0:
                    Console.WriteLine($"WARN: Trying to insert ({x}, {y}) point which is on one line with another: " +
                                      $"({node.X}, {node.Y}). Insertion is aborted.");
                    break;
            }

            node.SubtreeSize = 1 + Size(node.Left) + Size(node.Right);
            return node;
        }

        private static int ChooseHalfPlane(Node node, double x, double y, PlanePartitioning partitioning)
        {
            var (nodeX, nodeY) = node;
            return partitioning == PlanePartitioning.Horizontal
                ? nodeY.CompareTo(y)
                : nodeX.CompareTo(x);
        }

        public bool Contains(double x, double y)
        {
            return Contains(_root, x, y, PlanePartitioning.Vertical);
        }

        private static bool Contains(Node? node, double x, double y, PlanePartitioning partitioning)
        {
            while (true)
            {
                if (node is null) return false;
                switch (ChooseHalfPlane(node, x, y, partitioning))
                {
                    case < 0:
                        node = node.Right;
                        partitioning = partitioning.Flip();
                        continue;
                    case > 0:
                        node = node.Left;
                        partitioning = partitioning.Flip();
                        continue;
                    case 0:
                        return true;
                }
            }
        }

        public bool IsEmpty()
        {
            return Size() == 0;
        }

        public int Size()
        {
            return Size(_root);
        }

        private static int Size(Node? node)
        {
            return node?.SubtreeSize ?? 0;
        }

        [NotNull]
        private IEnumerable<(double, double)> Points()
        {
            var pairsOrderedByKey = new Queue<(double, double)>();
            TraverseInorder(_root, pairsOrderedByKey);
            return pairsOrderedByKey;
        }

        private static void TraverseInorder(Node? node, Queue<(double, double)> queue)
        {
            while (true)
            {
                if (node is null) return;
                TraverseInorder(node.Left, queue);
                queue.Enqueue((node.X, node.Y));
                node = node.Right;
            }
        }

        [NotNull]
        public IEnumerable<(double, double)> FindPointsInRectangle((double, double) p1,
                                                                   (double, double) p2,
                                                                   (double, double) p3,
                                                                   (double, double) p4)
        {
            var (x1, y1) = p1;
            var (x2, y2) = p2;
            var (x3, y3) = p3;
            var (x4, y4) = p4;
            AssertIsRectangle(p1, p2, p3, p4);
            var xCoordinates = new[] {x1, x2, x3, x4};
            var yCoordinates = new[] {y1, y2, y3, y4};
            var pointsInRectangle = new List<(double, double)>();
            FindPointsInRectangle(_root, xCoordinates.Max(), xCoordinates.Min(), yCoordinates.Max(), yCoordinates.Min(),
                PlanePartitioning.Vertical, pointsInRectangle);
            return pointsInRectangle;
        }

        private static void FindPointsInRectangle(Node? node, double xMax, double xMin, double yMax, double yMin,
                                                  PlanePartitioning partitioning,
                                                  IList<(double, double)> pointsInRectangle)
        {
            while (true)
            {
                if (node is null) return;
                var x = node.X;
                var y = node.Y;
                var isRectangleLeft = xMin < x;
                var isRectangleRight = x < xMax;
                var isRectangleBelow = yMin < y;
                var isRectangleAbove = y < yMax;
                var isInsideRectangle = isRectangleLeft && isRectangleRight && isRectangleBelow && isRectangleAbove;
                if (isInsideRectangle) pointsInRectangle.Add((x, y));
                var isRectangleInLeftSubtree = false;
                var isRectangleInRightSubtree = false;
                if (partitioning == PlanePartitioning.Vertical)
                {
                    if (isRectangleLeft) isRectangleInLeftSubtree = true;
                    if (isRectangleRight) isRectangleInRightSubtree = true;
                }
                else
                {
                    if (isRectangleBelow) isRectangleInLeftSubtree = true;
                    if (isRectangleAbove) isRectangleInRightSubtree = true;
                }

                var flipped = partitioning.Flip();
                if (isRectangleInLeftSubtree)
                    FindPointsInRectangle(node.Left, xMax, xMin, yMax, yMin, flipped, pointsInRectangle);
                if (isRectangleInRightSubtree)
                {
                    node = node.Right;
                    partitioning = flipped;
                    continue;
                }

                break;
            }
        }

        private static void AssertIsRectangle((double, double) p1,
                                              (double, double) p2,
                                              (double, double) p3,
                                              (double, double) p4)
        {
            var message = $"Points ({p1.Item1}, {p1.Item2}), ({p2.Item1}, {p2.Item2}), ({p3.Item1}, {p3.Item2})," +
                          $" ({p4.Item1}, {p4.Item2}) don't form a rectangle.";
            if (!IsRectangle(p1, p2, p3, p4)) throw new ArgumentException(message);
        }

        private static bool IsRectangle((double, double) p1,
                                        (double, double) p2,
                                        (double, double) p3,
                                        (double, double) p4)
        {
            return IsOrderedRectangle(p1, p2, p3, p4) || IsOrderedRectangle(p2, p3, p1, p4) ||
                   IsOrderedRectangle(p3, p1, p2, p4);
        }

        private static bool IsOrderedRectangle((double, double) p1, (double, double) p2, (double, double) p3,
                                               (double, double) p4)
        {
            return IsOrthogonal(p1, p2, p3) && IsOrthogonal(p2, p3, p4) && IsOrthogonal(p3, p4, p1);
        }

        private static bool IsOrthogonal((double, double) a, (double, double) b, (double, double) c)
        {
            return (b.Item1 - a.Item1) * (b.Item1 - c.Item1) + (b.Item2 - a.Item2) * (b.Item2 - c.Item2) == 0;
        }

        public (double, double)? FindNearestPoint(double x, double y)
        {
            if (_root is null) return null;
            return FindNearestPoint(_root, x, y, PlanePartitioning.Vertical);
        }

        private static (double, double) FindNearestPoint(Node node,
                                                         double x,
                                                         double y,
                                                         PlanePartitioning partitioning)
        {
            var compareResult = ChooseHalfPlane(node, x, y, partitioning);
            Node? firstHalfPlainNodeToTraverse;
            Node? secondHalfPlainNodeToTraverse;
            double currentNodeCoordinate;
            double specifiedPointCoordinate;
            if (partitioning == PlanePartitioning.Horizontal)
            {
                currentNodeCoordinate = node.Y;
                specifiedPointCoordinate = y;
            }
            else
            {
                currentNodeCoordinate = node.X;
                specifiedPointCoordinate = x;
            }

            switch (compareResult)
            {
                case < 0:
                    firstHalfPlainNodeToTraverse = node.Right;
                    secondHalfPlainNodeToTraverse = node.Left;
                    break;
                case > 0:
                    firstHalfPlainNodeToTraverse = node.Left;
                    secondHalfPlainNodeToTraverse = node.Right;
                    break;
                case 0:
                    if (Math.Abs(node.X - x) < double.Epsilon && Math.Abs(node.Y - y) < double.Epsilon) return (x, y);
                    firstHalfPlainNodeToTraverse = node.Left;
                    secondHalfPlainNodeToTraverse = node.Right;
                    break;
            }

            var champion = (node.X, node.Y);
            var championDelta = FindDistanceBetweenPoints(x, y, node.X, node.Y);
            var flippedPartitioning = partitioning.Flip();
            if (firstHalfPlainNodeToTraverse is not null)
            {
                var nearestPointInFirstHalfPlain =
                    FindNearestPoint(firstHalfPlainNodeToTraverse, x, y, flippedPartitioning);
                var deltaBetweenPartitionLineAndSpecifiedPoints =
                    Math.Abs(specifiedPointCoordinate - currentNodeCoordinate);
                var (nearestPointXInRightHalfPlain, nearestPointYInRightHalfPlain) = nearestPointInFirstHalfPlain;
                var deltaBetweenNearestInFirstHalfPlainAndSpecifiedPoints =
                    FindDistanceBetweenPoints(x, y, nearestPointXInRightHalfPlain, nearestPointYInRightHalfPlain);
                if (deltaBetweenNearestInFirstHalfPlainAndSpecifiedPoints < championDelta)
                {
                    championDelta = deltaBetweenNearestInFirstHalfPlainAndSpecifiedPoints;
                    champion = nearestPointInFirstHalfPlain;
                    var isNearestPointFromFirstHalfPlaneCloserThanCurrentPoint =
                        deltaBetweenNearestInFirstHalfPlainAndSpecifiedPoints <
                        deltaBetweenPartitionLineAndSpecifiedPoints;
                    if (isNearestPointFromFirstHalfPlaneCloserThanCurrentPoint) return nearestPointInFirstHalfPlain;
                }
            }

            if (secondHalfPlainNodeToTraverse is null) return champion;
            var nearestPointInSecondHalfPlain =
                FindNearestPoint(secondHalfPlainNodeToTraverse, x, y, flippedPartitioning);
            var (nearestPointXInLeftHalfPlain, nearestPointYInLeftHalfPlain) = nearestPointInSecondHalfPlain;
            var isNearestPointFromSecondHalfPlaneCloserThanCurrentChampionPoint =
                FindDistanceBetweenPoints(x, y, nearestPointXInLeftHalfPlain, nearestPointYInLeftHalfPlain) <
                championDelta;
            return isNearestPointFromSecondHalfPlaneCloserThanCurrentChampionPoint
                ? nearestPointInSecondHalfPlain
                : champion;
        }

        private static double FindDistanceBetweenPoints(double x, double y, double nearestX, double nearestY)
        {
            var deltaX = nearestX - x;
            var deltaY = nearestY - y;
            return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }

        private record Node (double X, double Y)
        {
            public Node? Left { get; set; }
            public Node? Right { get; set; }
            public int SubtreeSize { get; set; } = 1;
        }
    }

    internal static class PlanePartitioningExtensions
    {
        public static PlanePartitioning Flip(this PlanePartitioning grade)
        {
            return grade == PlanePartitioning.Horizontal ? PlanePartitioning.Vertical : PlanePartitioning.Horizontal;
        }
    }

    internal enum PlanePartitioning
    {
        Horizontal,
        Vertical
    }
}