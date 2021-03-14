using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace BST.SymbolTables
{
    [PublicAPI]
    public class LinearProbingHashTable<TKey, TValue> : ISymbolTable<TKey, TValue> where TKey : IEquatable<TKey>
    {
        private const int DefaultTableSize = 32;

        private KeyNode?[] _keys = new KeyNode?[DefaultTableSize];

        private record KeyNode (TKey Key);
        
        private record ValueNode (TValue Value);
        
        private int _size;

        private ValueNode?[] _values = new ValueNode?[DefaultTableSize];

        public LinearProbingHashTable()
        {
        }

        public LinearProbingHashTable(IDictionary<TKey, TValue> dictionary)
        {
            foreach (var (key, value) in dictionary) Add(key, value);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            for (var i = 0; i < _keys.Length; i++)
            {
                var key = _keys[i];
                if (key is null) continue;
                yield return KeyValuePair.Create(key.Key, _values[i]!.Value);
            }
        }

        public void Add(TKey key, TValue value)
        {
            var keysLength = _keys.Length;
            if (_size * 2 == keysLength) ResizeKeysAndValues(2 * keysLength);
            AddToKeysAndValues(key, value, _keys, _values);
            _size++;
        }

        public bool TryGet(TKey key, out TValue? value)
        {
            int j;
            var newSize = _keys.Length;
            for (j = Hash(key, newSize); _keys[j] is not null; j = (j + 1) % newSize)
            {
                if (!key.Equals(_keys[j]!.Key)) continue;
                value = _values[j]!.Value;
                return true;
            }

            value = default;
            return false;
        }

        public bool TryDelete(TKey key)
        {
            int j;
            var keySize = _keys.Length;
            for (j = Hash(key, keySize); _keys[j] is not null; j = (j + 1) % keySize)
            {
                if (!key.Equals(_keys[j])) continue;
                _keys[j] = null;
                _values[j] = default!;
                _size--;
                if (_size * 8 == keySize) ResizeKeysAndValues(keySize / 2);
                return true;
            }

            return false;
        }

        public bool Contains(TKey key) => TryGet(key, out _);

        public bool IsEmpty() => _size == 0;

        public int Size() => _size;

        public IEnumerable<TKey> Keys() => _keys.Where(node => node is not null).Select(node => node!.Key);

        public IEnumerable<TValue> Values() => _values.Where(node => node is not null).Select(node => node!.Value);

        private void ResizeKeysAndValues(int newSize)
        {
            var newKeys = new KeyNode?[newSize];
            var newValues = new ValueNode?[newSize];
            for (var i = 0; i < _keys.Length; i++)
            {
                var key = _keys[i];
                if (key is null) continue;
                AddToKeysAndValues(key.Key, _values[i]!.Value, newKeys, newValues);
            }

            _keys = newKeys;
            _values = newValues;
        }

        private static void AddToKeysAndValues(TKey key, TValue value, KeyNode?[] keys, ValueNode?[] values)
        {
            int j;
            var newSize = keys.Length;
            for (j = Hash(key, newSize); keys[j] is not null; j = (j + 1) % newSize)
                if (!key.Equals(keys[j]))
                    break;
            keys[j] = new KeyNode(key);
            values[j] = new ValueNode(value);
        }

        private static int Hash(TKey key, int tableSize) => Math.Abs(key.GetHashCode() & 0x7fffffff) % tableSize;
    }
}