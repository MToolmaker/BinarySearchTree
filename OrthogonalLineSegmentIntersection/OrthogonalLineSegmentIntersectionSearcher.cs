using System;
using System.Collections.Generic;
using System.Linq;
using BST.SymbolTables;
using JetBrains.Annotations;

namespace BST.OrthogonalLineSegmentIntersection
{
    [PublicAPI]
    public static class OrthogonalLineSegmentIntersectionSearcher
    {
        //TODO: Think about double
        public static IList<Point> Search(IEnumerable<ILine> lines)
        {
            var events = new List<KeyValuePair<int, ILine>>();
            foreach (var line in lines)
            {
                events.Add(KeyValuePair.Create(line.A.X, line));
                if (line.LineType == ILine.Type.Horizontal) events.Add(KeyValuePair.Create(line.B.X, line));
            }

            var eventsArray = events.ToArray();
            Array.Sort(eventsArray, (first, second) => first.Key < second.Key ? -1 : first.Key > second.Key ? 1 : 0);
            // TODO: We don't really need any key. UPDATE: Or do we?
            var intersections = new List<Point>();
            IOrderedSymbolTable<int, int> xToY = new SimpleBinarySearchTree<int, int>();
            foreach (var @event in eventsArray) HandleEvent(@event, xToY, intersections);
            return intersections;
        }

        private static void HandleEvent(KeyValuePair<int, ILine> @event,
                                        IOrderedSymbolTable<int, int> xToY,
                                        List<Point> intersections)
        {
            var (x, line) = @event;
            if (line.LineType == ILine.Type.Horizontal) HandleHorizontalLine(x, line, xToY);
            else HandleVerticalLine(line, xToY, intersections);
        }

        private static void HandleVerticalLine(ILine line, IOrderedSymbolTable<int, int> xToY, List<Point> intersections)
        {
            var aY = line.A.Y;
            var bY = line.B.Y;
            var intersectionWithCurrentVerticalLine = xToY
                .Range(Math.Min(aY, bY), Math.Max(aY, bY))
                .Select(intersection => CreatePointFrom(line.A.X, intersection.Key));
            intersections.AddRange(intersectionWithCurrentVerticalLine);
        }

        private static void HandleHorizontalLine(int x, ILine line, IOrderedSymbolTable<int, int> xToY)
        {
            if (x == line.A.X)
            {
                xToY.Add(line.A.Y, x);
                return;
            }

            var deleted = xToY.TryDelete(line.A.Y);
            if (deleted) return;
            var message = $"Reached end point of horizontal line but couldn't remove its y:{line.A.Y} coordinated";
            throw new InvalidOperationException(message);
        }

        public static Point CreatePointFrom(int x, int y) => new(x, y);

        public static ILine CreateLineFrom(Point a, Point b)
        {
            var sameColumn = a.X == b.X;
            var sameRow = a.Y == b.Y;
            if (sameColumn && sameRow) ThrowPointsAreSameException(a.X, a.Y);
            if (sameColumn) return new Line(a, b, ILine.Type.Vertical);
            if (sameRow) return new Line(a, b, ILine.Type.Horizontal);
            var message = $"Line is neither horizontal or vertical. Coordinates are ({a.X},{a.Y}) and ({b.X},{b.Y})";
            throw new InvalidOperationException(message);
        }

        private static void ThrowPointsAreSameException(int x, int y)
        {
            throw new InvalidOperationException($"Line cannot be created for coincident points: ({x},{y})");
        }
    }

    public record Point(int X, int Y);

    public interface ILine
    {
        public enum Type
        {
            Vertical,
            Horizontal
        }

        Point A { get; }
        Point B { get; }
        Type LineType { get; }
    }

    internal record Line(Point A, Point B, ILine.Type LineType) : ILine;
}