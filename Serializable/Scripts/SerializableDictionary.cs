using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SimTheory {

    [Serializable]
    public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver, IDictionary<TKey, TValue> {

        [SerializeField] private List<KeyValueEntry> entries = new List<KeyValueEntry>();

        private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

        [Serializable]
        private class KeyValueEntry {
            public TKey key;
            public TValue value;
        }

        public void OnBeforeSerialize() {
            entries = dictionary.Select(kvp => new KeyValueEntry { key = kvp.Key, value = kvp.Value }).ToList();
        }

        public void OnAfterDeserialize() {
            dictionary.Clear();
            foreach (var entry in entries) {
                if (entry.key != null && !dictionary.ContainsKey(entry.key)) {
                    dictionary.Add(entry.key, entry.value);
                }
            }
        }

        public TValue this[TKey key] {
            get => dictionary.TryGetValue(key, out var value) ? value : default;
            set => dictionary[key] = value;
        }

        public ICollection<TKey> Keys => dictionary.Keys;
        public ICollection<TValue> Values => dictionary.Values;
        public int Count => dictionary.Count;
        public bool IsReadOnly => false;

        public void Add(TKey key, TValue value) => dictionary.Add(key, value);
        public void Add(KeyValuePair<TKey, TValue> item) => dictionary.Add(item.Key, item.Value);
        public void Clear() => dictionary.Clear();
        public bool Contains(KeyValuePair<TKey, TValue> item) => dictionary.Contains(item);
        public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);
        public bool Remove(TKey key) => dictionary.Remove(key);
        public bool Remove(KeyValuePair<TKey, TValue> item) => dictionary.Remove(item.Key);
        public bool TryGetValue(TKey key, out TValue value) => dictionary.TryGetValue(key, out value);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => dictionary.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
            (dictionary as IDictionary<TKey, TValue>).CopyTo(array, arrayIndex);
        }




        // Implicit cast to Dictionary<TKey, TValue>
        public static implicit operator Dictionary<TKey, TValue>(SerializableDictionary<TKey, TValue> serializableDict) {
            return serializableDict?.dictionary;
        }

        // Explicit cast from Dictionary<TKey, TValue>
        public static explicit operator SerializableDictionary<TKey, TValue>(Dictionary<TKey, TValue> dict) {
            var serializableDict = new SerializableDictionary<TKey, TValue>();
            if (dict != null) {
                foreach (var kvp in dict) {
                    serializableDict.Add(kvp.Key, kvp.Value);
                }
            }
            return serializableDict;
        }
    }
}