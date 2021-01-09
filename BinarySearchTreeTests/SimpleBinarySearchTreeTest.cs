using System.Collections.Generic;
using BinarySearchTree.SymbolTables;
using NUnit.Framework;

namespace BinarySearchTreeTests
{
    public class SimpleBinarySearchTreeTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TryGetOperationTest()
        {
            var numberToString = FillSimpleBinarySearchTreeToTest();
            var expectedNumberToString = FillExpectedMap();
            for (var number = 1; number <= 3; number++)
            {
                Assert.True(expectedNumberToString.TryGetValue(number, out var expected));
                Assert.True(numberToString.TryGet(number, out var actual));
                Assert.AreEqual(expected, actual);
            }
        }
        
        [Test]
        public void ContainsOperationTest()
        {
            var numberToString = FillSimpleBinarySearchTreeToTest();
            for (var number = 1; number <= 3; number++)
            {
                Assert.True(numberToString.Contains(number));
            }
        }

        private static SimpleBinarySearchTree<int, string> FillSimpleBinarySearchTreeToTest()
        {
            return new() {{3, "A"}, {1, "B"}, {2, "C"}};
        }

        private static Dictionary<int, string> FillExpectedMap()
        {
            return new() {{3, "A"}, {1, "B"}, {2, "C"}};
        }
    }
}