using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using BST.SymbolTables;
using NUnit.Framework;

namespace BinarySearchTreeTests
{
    public class SymbolTablesTest
    {
        private static readonly ImmutableDictionary<int, string> ExpectedNumberToString =
            new Dictionary<int, string> {{3, "A"}, {1, "B"}, {2, "C"}, {5, "E"}}.ToImmutableDictionary();

        private static readonly IEnumerable<ISymbolTable<int, string>> SymbolTables =
            new ISymbolTable<int, string>[]
            {
                new SimpleBinarySearchTree<int, string>(ExpectedNumberToString),
                new RedBlackBinarySearchTree<int, string>(ExpectedNumberToString)
            };

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TryGetOperationTest()
        {
            foreach (var numberToString in SymbolTables)
            foreach (var number in ExpectedNumberToString.Keys)
            {
                Assert.True(numberToString.TryGet(number, out var actual));
                Assert.AreEqual(ExpectedNumberToString[number], actual);
            }
        }

        [Test]
        public void ContainsOperationTest()
        {
            foreach (var numberToString in SymbolTables)
            foreach (var number in ExpectedNumberToString.Keys)
                Assert.True(numberToString.Contains(number));
        }

        [Test]
        public void KeysOperationTest()
        {
            foreach (var numberToString in SymbolTables)
            foreach (var number in numberToString.Keys())
                Assert.True(ExpectedNumberToString.ContainsKey(number));
        }

        [Test]
        public void SizeOperationTest()
        {
            foreach (var numberToString in SymbolTables)
                Assert.AreEqual(ExpectedNumberToString.Count, numberToString.Size());
        }

        [Test]
        public void ValuesOperationTest()
        {
            foreach (var numberToString in SymbolTables)
            foreach (var @string in numberToString.Values())
                Assert.True(ExpectedNumberToString.ContainsValue(@string));
        }

        [Test]
        public void EmptyOperationTest()
        {
            foreach (var numberToString in SymbolTables) Assert.True(!numberToString.IsEmpty());
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
        public void TryDeleteOperationForRedBlackIsNotImplementedTest()
        {
            RedBlackBinarySearchTree<int, string> bst = new() {{3, "A"}, {1, "B"}, {2, "C"}, {5, "E"}};
            Assert.Throws<NotImplementedException>(() => bst.TryDelete(1));
        }
    }
}