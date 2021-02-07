using System.Collections.Generic;
using BST.OrthogonalLineSegmentIntersection;
using NUnit.Framework;

namespace BinarySearchTreeTests
{
    public class OrthogonalLineSegmentIntersectionSearcherTest
    {
        [Test]
        public void TryGetOperationTest()
        {
            var a = OrthogonalLineSegmentIntersectionSearcher.CreatePointFrom(0, 0);
            var b = OrthogonalLineSegmentIntersectionSearcher.CreatePointFrom(5, 0);
            var c = OrthogonalLineSegmentIntersectionSearcher.CreatePointFrom(1, 1);
            var d = OrthogonalLineSegmentIntersectionSearcher.CreatePointFrom(1, -1);
            var e = OrthogonalLineSegmentIntersectionSearcher.CreatePointFrom(4, 1);
            var f = OrthogonalLineSegmentIntersectionSearcher.CreatePointFrom(4, -1);
            var first = OrthogonalLineSegmentIntersectionSearcher.CreateLineFrom(a, b);
            var second = OrthogonalLineSegmentIntersectionSearcher.CreateLineFrom(c, d);
            var third = OrthogonalLineSegmentIntersectionSearcher.CreateLineFrom(e, f);
            var lines = new[] {first, second, third};
            var actualIntersections = new HashSet<Point>(OrthogonalLineSegmentIntersectionSearcher.Search(lines));
            var expectedIntersections = new HashSet<Point>
            {
                OrthogonalLineSegmentIntersectionSearcher.CreatePointFrom(1, 0),
                OrthogonalLineSegmentIntersectionSearcher.CreatePointFrom(4, 0)
            };
            Assert.True(actualIntersections.SetEquals(expectedIntersections));
        }
    }
}