using System.Collections.Generic;
using BinarySearchTree.SymbolTables;
using NUnit.Framework;

namespace BinarySearchTreeTests
{
    public class SimpleBinarySearchTreeTest
    {
        private readonly SimpleBinarySearchTree<int, string> _numberToString = new() {{3, "A"}, {1, "B"}, {2, "C"}};
        private readonly Dictionary<int, string> _expectedNumberToString = new() {{3, "A"}, {1, "B"}, {2, "C"}};
        
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TryGetOperationTest()
        {
            for (var number = 1; number <= 3; number++)
            {
                Assert.True(_expectedNumberToString.TryGetValue(number, out var expected));
                Assert.True(_numberToString.TryGet(number, out var actual));
                Assert.AreEqual(expected, actual);
            }
        }
        
        [Test]
        public void ContainsOperationTest()
        {
            for (var number = 1; number <= 3; number++) Assert.True(_numberToString.Contains(number));
        }
    }
}