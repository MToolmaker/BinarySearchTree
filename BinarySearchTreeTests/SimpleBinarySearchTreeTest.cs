using System.Collections.Generic;
using BinarySearchTree.SymbolTables;
using NUnit.Framework;

namespace BinarySearchTreeTests
{
    public class SimpleBinarySearchTreeTest
    {
        private static readonly SimpleBinarySearchTree<int, string> NumberToString =
            new() {{3, "A"}, {1, "B"}, {2, "C"}};
        private static readonly Dictionary<int, string> ExpectedNumberToString =
            new() {{3, "A"}, {1, "B"}, {2, "C"}};
        
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TryGetOperationTest()
        {
            for (var number = 1; number <= 3; number++)
            {
                Assert.True(ExpectedNumberToString.TryGetValue(number, out var expected));
                Assert.True(NumberToString.TryGet(number, out var actual));
                Assert.AreEqual(expected, actual);
            }
        }
        
        [Test]
        public void ContainsOperationTest()
        {
            for (var number = 1; number <= 3; number++) Assert.True(NumberToString.Contains(number));
        }
    }
}