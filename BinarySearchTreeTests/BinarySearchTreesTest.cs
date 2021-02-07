using System.Collections.Generic;
using System.Collections.Immutable;
using BinarySearchTree.SymbolTables;
using NUnit.Framework;

namespace BinarySearchTreeTests
{
    public class BinarySearchTreesTest
    {
        private static readonly ImmutableDictionary<int, string> ExpectedNumberToString =
            new Dictionary<int, string> {{3, "A"}, {1, "B"}, {2, "C"}, {5, "E"}}.ToImmutableDictionary();

        private static readonly IEnumerable<IOrderedSymbolTable<int, string>> BinarySearchTrees =
            new IOrderedSymbolTable<int, string>[]
            {
                new SimpleBinarySearchTree<int, string> {{3, "A"}, {1, "B"}, {2, "C"}, {5, "E"}},
                new RedBlackBinarySearchTree<int, string> {{3, "A"}, {1, "B"}, {2, "C"}, {5, "E"}}
            };

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TryGetOperationTest()
        {
            foreach (var numberToString in BinarySearchTrees)
            foreach (var number in ExpectedNumberToString.Keys)
            {
                Assert.True(numberToString.TryGet(number, out var actual));
                Assert.AreEqual(ExpectedNumberToString[number], actual);
            }
        }

        [Test]
        public void ContainsOperationTest()
        {
            foreach (var numberToString in BinarySearchTrees)
            foreach (var number in ExpectedNumberToString.Keys)
                Assert.True(numberToString.Contains(number));
        }

        [Test]
        public void KeysOperationTest()
        {
            foreach (var numberToString in BinarySearchTrees)
            foreach (var number in numberToString.Keys())
                Assert.True(ExpectedNumberToString.ContainsKey(number));
        }

        [Test]
        public void OrderedKeysOperationTest()
        {
            foreach (var numberToString in BinarySearchTrees)
            {
                var previousKey = -1;
                foreach (var currentKey in numberToString.OrderedKeys())
                {
                    Assert.True(previousKey <= currentKey);
                    previousKey = currentKey;
                }
            }
        }

        [Test]
        public void SizeOperationTest()
        {
            foreach (var numberToString in BinarySearchTrees)
                Assert.AreEqual(ExpectedNumberToString.Count, numberToString.Size());
        }

        [Test]
        public void ValuesOperationTest()
        {
            foreach (var numberToString in BinarySearchTrees)
            foreach (var @string in numberToString.Values())
                Assert.True(ExpectedNumberToString.ContainsValue(@string));
        }

        [Test]
        public void EmptyOperationTest()
        {
            foreach (var numberToString in BinarySearchTrees) Assert.True(!numberToString.IsEmpty());
        }

        [Test]
        public void TryGetMinOperationTest()
        {
            foreach (var numberToString in BinarySearchTrees)
            {
                Assert.True(numberToString.TryGetMin(out var min));
                Assert.AreEqual(1, min);
            }
        }

        [Test]
        public void TryGetMaxOperationTest()
        {
            foreach (var numberToString in BinarySearchTrees)
            {
                Assert.True(numberToString.TryGetMax(out var max));
                Assert.AreEqual(5, max);
            }
        }

        [Test]
        public void TryGetFloorOperationTest()
        {
            foreach (var numberToString in BinarySearchTrees)
            {
                Assert.True(numberToString.TryGetFloor(4, out var floor));
                Assert.AreEqual(3, floor);
            }
        }

        [Test]
        public void TryGetCeilingOperationTest()
        {
            foreach (var numberToString in BinarySearchTrees)
            {
                Assert.True(numberToString.TryGetCeiling(4, out var floor));
                Assert.AreEqual(5, floor);
            }
        }

        [Test]
        public void RankOperationTest()
        {
            foreach (var numberToString in BinarySearchTrees) Assert.AreEqual(3, numberToString.Rank(4));
        }

        [Test]
        public void TryDeleteOperationForSimpleTreeTest()
        {
            SimpleBinarySearchTree<int, string> bst = new() {{3, "A"}, {1, "B"}, {2, "C"}, {5, "E"}};
            foreach (var number in ExpectedNumberToString.Keys)
            {
                Assert.True(bst.TryDelete(number));
                Assert.True(!bst.Contains(number));
            }

            Assert.True(bst.IsEmpty());
        }
        
        [Test]
        public void RangeCountOperationTest()
        {
            foreach (var numberToString in BinarySearchTrees)
            {
                Assert.AreEqual(3, numberToString.RangeCount(1, 3));
                Assert.AreEqual(1, numberToString.RangeCount(4, 5));
                Assert.AreEqual(1, numberToString.RangeCount(3, 4));
                Assert.AreEqual(2, numberToString.RangeCount(2, 4));
                Assert.AreEqual(4, numberToString.RangeCount(0, 7));
                Assert.AreEqual(0, numberToString.RangeCount(6, 7));
            }
        }
    }
}