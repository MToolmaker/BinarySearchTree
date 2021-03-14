using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace BST.SymbolTables
{
    [PublicAPI]
    public class SeparateChainingHashTable<TKey, TValue> : ISymbolTable<TKey, TValue> where TKey : IEquatable<TKey>
    {
        private const int DefaultTableSize = 32;

        private int _size;

        private Node?[] _table = new Node[DefaultTableSize];

        public SeparateChainingHashTable(IDictionary<TKey, TValue> dictionary)
        {
            foreach (var (key, value) in dictionary) Add(key, value);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (var node in _table)
                for (var currentNode = node; currentNode is not null; currentNode = currentNode.Next)
                    yield return KeyValuePair.Create(currentNode.Key, currentNode.Value);
        }

        public void Add(TKey key, TValue value)
        {
            var tableSize = _table.Length;
            var hash = Hash(key, tableSize);
            var node = _table[hash];
            for (var currentNode = node; currentNode is not null; currentNode = currentNode.Next)
            {
                if (!currentNode.Key.Equals(key)) continue;
                currentNode.Value = value;
                return;
            }

            if (_size == tableSize * 5)
            {
                ResizeTableWith(2 * tableSize);
                hash = Hash(key, _table.Length);
            }

            _table[hash] = new Node(key, value, node);
            _size++;
        }

        public bool TryGet(TKey key, out TValue? value)
        {
            var hash = Hash(key, _table.Length);
            var node = _table[hash];
            for (var currentNode = node; currentNode is not null; currentNode = currentNode.Next)
            {
                if (!currentNode.Key.Equals(key)) continue;
                value = currentNode.Value;
                return true;
            }

            value = default;
            return false;
        }

        public bool TryDelete(TKey key)
        {
            var hash = Hash(key, _table.Length);
            var node = _table[hash];
            if (node is null) return false;
            if (node.Key.Equals(key))
            {
                _table[hash] = node.Next;
                _size--;
                ShrinkTableIfSparse();
                return true;
            }

            // ReSharper disable once UseDeconstruction
            var previous = node;
            for (var currentNode = previous.Next; currentNode is not null; currentNode = currentNode.Next)
            {
                if (!currentNode.Key.Equals(key)) continue;
                previous.Next = currentNode.Next;
                _size--;
                ShrinkTableIfSparse();
                return true;
            }

            return false;
        }

        public bool Contains(TKey key) => TryGet(key, out _);

        public bool IsEmpty() => _size == 0;

        public int Size() => _size;

        public IEnumerable<TKey> Keys() => this.Select(pair => pair.Key);

        public IEnumerable<TValue> Values() => this.Select(pair => pair.Value);

        private void ResizeTableWith(int newSize)
        {
            var newTable = new Node[newSize];
            foreach (var node in _table)
                for (var currentNode = node; currentNode is not null; currentNode = currentNode.Next)
                    AddToTable(currentNode.Key, currentNode.Value, newTable);
            _table = newTable;
        }

        private static void AddToTable(TKey key, TValue value, Node[] table)
        {
            var hash = Hash(key, table.Length);
            var node = table[hash];
            for (var currentNode = node; currentNode is not null; currentNode = currentNode.Next)
            {
                if (!currentNode.Key.Equals(key)) continue;
                currentNode.Value = value;
                return;
            }

            table[hash] = new Node(key, value, node);
        }

        private void ShrinkTableIfSparse()
        {
            var tableSize = _table.Length;
            if (_size == tableSize / 4) ResizeTableWith(tableSize / 2);
        }

        private static int Hash(TKey key, int tableSize) => Math.Abs(key.GetHashCode() & 0x7fffffff) % tableSize;

        private record Node (TKey Key, TValue Value, Node? Next)
        {
            public TValue Value { get; set; } = Value;
            public Node? Next { get; set; } = Next;
        }
    }
}