using BST.IntervalTree;
using NUnit.Framework;

namespace BinarySearchTreeTests
{
    public class IntervalTreeTest
    {
        private static readonly (double, double)[] Intervals =
        {
            (0, 9),
            (1, 5),
            (7, 8),
            (10, 12),
            (11, 15),
            (14, 18)
        };

        [Test]
        public void FindIntersectionOperationTest()
        {
            var intervals = new IntervalTree();
            foreach (var interval in Intervals) intervals.Add(interval);
            CollectionAssert.Contains(new[] {(0, 9), (1, 5)}, intervals.FindIntersection(3, 6));
            CollectionAssert.Contains(new[] {(0, 9), (1, 5)}, intervals.FindIntersection(3, 6));
            CollectionAssert.Contains(new[] {(0, 9), (1, 5), (10, 12), (11, 15)},
                intervals.FindIntersection(4, 13));
        }
        
        [Test]
        public void FindIntersectionsOperationTest()
        {
            var intervals = new IntervalTree();
            foreach (var interval in Intervals) intervals.Add(interval);
            CollectionAssert.AreEquivalent(new[] {(0, 9), (1, 5)}, intervals.FindIntersections(3, 6));
            CollectionAssert.AreEquivalent(new[] {(0, 9), (1, 5)}, intervals.FindIntersections(3, 6));
            CollectionAssert.AreEquivalent(new[] {(0, 9), (1, 5), (10, 12), (11, 15)},
                intervals.FindIntersections(4, 13));
        }
    }
}