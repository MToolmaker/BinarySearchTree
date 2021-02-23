using System;
using System.Collections;
using BST.KdTree;
using NUnit.Framework;

namespace BinarySearchTreeTests
{
    public class TwoDimensionalTreeTest
    {
        private static readonly (double, double)[] Points =
        {
            (4.0, 4.0),
            (2.0, 3.0),
            (1.0, 1.0),
            (8.0, 2.0),
            (7.0, 6.0)
        };

        [Test]
        public void FindNearestPointOperationTest()
        {
            var twoDimensionalTree = new TwoDimensionalTree();
            foreach (var (x, y) in Points) twoDimensionalTree.Add(x, y);
            CheckNearestPoint(twoDimensionalTree, (7.0, 7.0), (7.0, 6.0));
            CheckNearestPoint(twoDimensionalTree, (2.0, 2.0), (2.0, 3.0));
            CheckNearestPoint(twoDimensionalTree, (2.0, 4.0), (2.0, 3.0));
            CheckNearestPoint(twoDimensionalTree, (6.0, 1.0), (8.0, 2.0));
            CheckNearestPoint(twoDimensionalTree, (-6.0, -1.0), (1.0, 1.0));
        }

        [Test]
        public void FindPointsInRectangleOperationTest()
        {
            var twoDimensionalTree = new TwoDimensionalTree();
            foreach (var (x, y) in Points) twoDimensionalTree.Add(x, y);
            CheckPointsInRectangle(twoDimensionalTree, (0, 5), (0, 0), (3, 5), (3, 0), new[] {(1, 1), (2, 3)});
            CheckPointsInRectangle(twoDimensionalTree, (5, -1), (9, -1), (5, 3), (9, 3), new[] {(8, 2)});
            CheckPointsInRectangle(twoDimensionalTree, (-10, -10), (-10, 10), (10, 10), (10, -10), Points);
            CheckPointsInRectangle(twoDimensionalTree, (3, 3), (3, 8), (8, 3), (8, 8), new[] {(4, 4), (7, 6)});
            CheckPointsDontFormRectangleException(twoDimensionalTree);
        }

        private static void CheckPointsDontFormRectangleException(TwoDimensionalTree twoDimensionalTree)
        {
            try
            {
                CheckPointsInRectangle(twoDimensionalTree, (3, 3), (3, 8), (8, 3), (8, 7), new[] {(4, 4), (7, 6)});
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        private static void CheckPointsInRectangle(TwoDimensionalTree twoDimensionalTree,
                                                   (int, int) p1,
                                                   (int, int) p2,
                                                   (int, int) p3,
                                                   (int, int) p4,
                                                   IEnumerable expectedPoints)
        {
            var pointsInRectangle = twoDimensionalTree.FindPointsInRectangle(p1, p2, p3, p4);
            CollectionAssert.AreEquivalent(expectedPoints, pointsInRectangle);
        }

        private static void CheckNearestPoint(TwoDimensionalTree twoDimensionalTree,
                                              (double, double) specified,
                                              (double, double) expected)
        {
            var (specifiedX, specifiedY) = specified;
            var nearestPoint = twoDimensionalTree.FindNearestPoint(specifiedX, specifiedY);
            if (nearestPoint is null)
            {
                Assert.Fail("Two dimensional tree is empty.");
                return;
            }

            var (expectedX, expectedY) = expected;
            var message = $"Fail. Found closest point is ({nearestPoint.Value.Item1}, {nearestPoint.Value.Item2}). " +
                          $"It is different from expected ({expectedX}, {expectedY}).";
            Assert.AreEqual(expected, nearestPoint, message);
        }
    }
}