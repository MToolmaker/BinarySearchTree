using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using BST.SymbolTables;
using NUnit.Framework;

namespace BinarySearchTreeTests
{
    public class OrderedSymbolTablesTests
    {
        private static readonly ImmutableDictionary<int, string> ExpectedNumberToString =
            new Dictionary<int, string> {{3, "A"}, {1, "B"}, {2, "C"}, {5, "E"}}.ToImmutableDictionary();

        private static readonly IEnumerable<IOrderedSymbolTable<int, string>> OrderedSymbolTables =
            new IOrderedSymbolTable<int, string>[]
            {
                new SimpleBinarySearchTree<int, string>(ExpectedNumberToString),
                new RedBlackBinarySearchTree<int, string>(ExpectedNumberToString)
            };

        [Test]
        public void OrderedKeysOperationTest()
        {
            foreach (var numberToString in OrderedSymbolTables)
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
        public void TryGetMinOperationTest()
        {
            foreach (var numberToString in OrderedSymbolTables)
            {
                Assert.True(numberToString.TryGetMin(out var min));
                Assert.AreEqual(1, min);
            }
        }


        [Test]
        public void TryGetMaxOperationTest()
        {
            foreach (var numberToString in OrderedSymbolTables)
            {
                Assert.True(numberToString.TryGetMax(out var max));
                Assert.AreEqual(5, max);
            }
        }

        [Test]
        public void TryGetFloorOperationTest()
        {
            foreach (var numberToString in OrderedSymbolTables)
            {
                Assert.True(numberToString.TryGetFloor(4, out var floor));
                Assert.AreEqual(3, floor);
            }
        }

        [Test]
        public void TryGetCeilingOperationTest()
        {
            foreach (var numberToString in OrderedSymbolTables)
            {
                Assert.True(numberToString.TryGetCeiling(4, out var floor));
                Assert.AreEqual(5, floor);
            }
        }

        [Test]
        public void RankOperationTest()
        {
            foreach (var numberToString in OrderedSymbolTables) Assert.AreEqual(3, numberToString.Rank(4));
        }

        [Test]
        public void RangeCountOperationTest()
        {
            foreach (var numberToString in OrderedSymbolTables)
            {
                Assert.AreEqual(3, numberToString.RangeCount(1, 3));
                Assert.AreEqual(1, numberToString.RangeCount(4, 5));
                Assert.AreEqual(1, numberToString.RangeCount(3, 4));
                Assert.AreEqual(2, numberToString.RangeCount(2, 4));
                Assert.AreEqual(4, numberToString.RangeCount(0, 7));
                Assert.AreEqual(0, numberToString.RangeCount(6, 7));
            }
        }

        [Test]
        public void RangeOperationTest()
        {
            foreach (var numberToString in OrderedSymbolTables)
            {
                TestRangeFromTwoToFour(numberToString);
                TestRangeFromThreeToFour(numberToString);
                TestRangeFromFourToFour(numberToString);
                TestRangeFromFourToFive(numberToString);
            }
        }

        private static void TestRangeFromFourToFive(IOrderedSymbolTable<int, string> numberToString)
        {
            var elementsWithFourToFiveKeys = numberToString.Range(4, 5).ToList();
            Assert.True(elementsWithFourToFiveKeys.Contains(KeyValuePair.Create(5, "E")));
            Assert.AreEqual(1, elementsWithFourToFiveKeys.Count);
        }

        private static void TestRangeFromFourToFour(IOrderedSymbolTable<int, string> numberToString)
        {
            Assert.AreEqual(0, numberToString.Range(4, 4).ToList().Count);
        }

        private static void TestRangeFromThreeToFour(IOrderedSymbolTable<int, string> numberToString)
        {
            var elementsWithThreeToFourKeys = numberToString.Range(3, 4).ToList();
            Assert.True(elementsWithThreeToFourKeys.Contains(KeyValuePair.Create(3, "A")));
            Assert.AreEqual(1, elementsWithThreeToFourKeys.Count);
        }

        private static void TestRangeFromTwoToFour(IOrderedSymbolTable<int, string> numberToString)
        {
            var elementsWithTwoToFourKeys = numberToString.Range(2, 4).ToList();
            Assert.True(elementsWithTwoToFourKeys.Contains(KeyValuePair.Create(2, "C")));
            Assert.True(elementsWithTwoToFourKeys.Contains(KeyValuePair.Create(3, "A")));
            Assert.AreEqual(2, elementsWithTwoToFourKeys.Count);
        }
    }
}