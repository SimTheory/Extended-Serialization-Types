using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace SimTheory {
    [Serializable]
    public class SerializableHashSet<T> : IEnumerable<T>, ICollection<T> {
        [SerializeField]
        List<T> items = new List<T>();





        #region Constructors
        public SerializableHashSet() { }

        public SerializableHashSet(IEnumerable<T> collection) {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            foreach (var item in collection) {
                if (!items.Contains(item)) {
                    items.Add(item);
                }
            }
        }
        #endregion



        public T this[int index] {
            get {
                if (index < 0 || index >= items.Count)
                    throw new ArgumentOutOfRangeException(nameof(index));
                return items[index];
            }
            set {
                if (index < 0 || index >= items.Count)
                    throw new ArgumentOutOfRangeException(nameof(index));
                if (items.Contains(value) && !EqualityComparer<T>.Default.Equals(items[index], value))
                    throw new ArgumentException("An item with the same value already exists in the set.");
                items[index] = value;
            }
        }

        public bool Add(T item) {
            if (items.Contains(item)) return false;
            items.Add(item);
            return true;
        }

        public bool AddAt(T item, int index) {
            if (items.Contains(item)) return false;
            if (index < 0 || index > items.Count) throw new ArgumentOutOfRangeException(nameof(index));
            items.Insert(index, item);
            return true;
        }

        public bool Remove(T item) {
            return items.Remove(item);
        }

        public int RemoveAll(Predicate<T> match) {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            int initialCount = items.Count;
            items.RemoveAll(match);
            return initialCount - items.Count;
        }

        public bool Contains(T item) {
            return items.Contains(item);
        }

        public void Clear() {
            items.Clear();
        }

        public IReadOnlyList<T> Items => items.AsReadOnly();

        public int Count => items.Count;

        public bool IsReadOnly => false;

        public IEnumerator<T> GetEnumerator() {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public T Find(Func<T, bool> predicate) {
            return items.FirstOrDefault(predicate);
        }

        void ICollection<T>.Add(T item) {
            Add(item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (array.Length - arrayIndex < items.Count)
                throw new ArgumentException("The number of elements in the source is greater than the available space from arrayIndex to the end of the destination array.");

            items.CopyTo(array, arrayIndex);
        }

        public T[] ToArray() {
            return items.ToArray();
        }


        // Implicit cast to HashSet<T>
        public static implicit operator HashSet<T>(SerializableHashSet<T> serializableSet) {
            return serializableSet == null ? null : new HashSet<T>(serializableSet.items);
        }

        // Explicit cast from HashSet<T>
        public static explicit operator SerializableHashSet<T>(HashSet<T> hashSet) {
            return hashSet == null ? null : new SerializableHashSet<T>(hashSet);
        }
    }
}