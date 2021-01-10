using System.Collections.Generic;
using BinarySearchTree.SymbolTables;
using NUnit.Framework;

namespace BinarySearchTreeTests
{
    public class SimpleBinarySearchTreeTest
    {
        private static readonly SimpleBinarySearchTree<int, string> NumberToString =
            new() {{3, "A"}, {1, "B"}, {2, "C"}, {5, "E"}};

        private static readonly Dictionary<int, string> ExpectedNumberToString =
            new() {{3, "A"}, {1, "B"}, {2, "C"}, {5, "E"}};

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TryGetOperationTest()
        {
            foreach (var number in ExpectedNumberToString.Keys)
            {
                Assert.True(NumberToString.TryGet(number, out var actual));
                Assert.AreEqual(ExpectedNumberToString[number], actual);
            }
        }

        [Test]
        public void ContainsOperationTest()
        {
            foreach (var number in ExpectedNumberToString.Keys)
            {
                Assert.True(NumberToString.Contains(number));
            }
        }

        [Test]
        public void KeysOperationTest()
        {
            foreach (var number in NumberToString.Keys()) Assert.True(ExpectedNumberToString.ContainsKey(number));
        }

        [Test]
        public void OrderedKeysOperationTest()
        {
            var previousKey = -1;
            foreach (var currentKey in NumberToString.OrderedKeys())
            {
                Assert.True(previousKey <= currentKey);
                previousKey = currentKey;
            }
        }

        [Test]
        public void SizeOperationTest()
        {
            Assert.AreEqual(ExpectedNumberToString.Count, NumberToString.Size());
        }

        [Test]
        public void ValuesOperationTest()
        {
            foreach (var @string in NumberToString.Values()) Assert.True(ExpectedNumberToString.ContainsValue(@string));
        }

        [Test]
        public void EmptyOperationTest()
        {
            Assert.True(!NumberToString.IsEmpty());
        }
        
        [Test]
        public void TryGetMinOperationTest()
        {
            Assert.True(NumberToString.TryGetMin(out var min));
            Assert.AreEqual(1, min);
        }
        
        [Test]
        public void TryGetMaxOperationTest()
        {
            Assert.True(NumberToString.TryGetMax(out var max));
            Assert.AreEqual(5, max);
        }
        
        [Test]
        public void TryGetFloorOperationTest()
        {
            Assert.True(NumberToString.TryGetFloor(4, out var floor));
            Assert.AreEqual(3, floor);
        }
        
        [Test]
        public void TryGetCeilingOperationTest()
        {
            Assert.True(NumberToString.TryGetCeiling(4, out var floor));
            Assert.AreEqual(5, floor);
        }
        
        [Test]
        public void RankOperationTest()
        {
            Assert.AreEqual(3, NumberToString.Rank(4));
        }
        
        [Test]
        public void TryDeleteOperationTest()
        {
            SimpleBinarySearchTree<int, string> bst = new() {{3, "A"}, {1, "B"}, {2, "C"}, {5, "E"}};
            foreach (var number in ExpectedNumberToString.Keys)
            {
                Assert.True(bst.TryDelete(number));
                Assert.True(!bst.Contains(number));
            }
            Assert.True(bst.IsEmpty());
        }
    }
}