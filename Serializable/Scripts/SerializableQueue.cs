using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimTheory {
    [Serializable]
    public class SerializableQueue<T> : IEnumerable<T> {

        [SerializeField]
        List<T> items = new List<T>();
        Queue<T> queue = new Queue<T>();



        public SerializableQueue() {
            SyncQueueFromList();
        }
        public SerializableQueue(IEnumerable<T> collection) {
            if (collection != null) {
                items.AddRange(collection);
                SyncQueueFromList();
            }
        }



        public T this[int index] {
            get {
                if (index < 0 || index >= queue.Count)
                    throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
                return items[index];
            }
        }



        public int Count => queue.Count;
        public bool IsEmptyOrNull => queue == null || queue.Count <= 0;



        public void Enqueue(T item) {
            queue.Enqueue(item);
            items.Add(item);
        }
        public void Enqueue(IEnumerable enums) {
            foreach (T item in enums) {
                Enqueue(item);
                items.Add(item);
            }
        }
        public T Dequeue() {

            if (queue.TryDequeue(out T result)) {
                if (items.Count > 0)
                    items.RemoveAt(0);
                return result;
            }
            return default;
        }
        public T PeekFirst() {
            if (queue.Count == 0) {
                return default;
            }
            return queue.Peek();
        }
        public T PeekLast() {
            if (queue.Count == 0) {
                return default;
            }
            return items[items.Count - 1];
        }
        public void OnAfterDeserialize() {
            SyncQueueFromList();
        }
        private void SyncQueueFromList() {
            queue.Clear();
            foreach (var item in items) {
                if (!queue.Contains(item)) {
                    queue.Enqueue(item);
                }
            }
        }
        public bool Remove(T item) {
            if (queue.Contains(item)) {
                List<T> tempList = new List<T>();

                while (queue.Count > 0) {
                    T dequeuedItem = queue.Dequeue();
                    if (!EqualityComparer<T>.Default.Equals(dequeuedItem, item)) {
                        tempList.Add(dequeuedItem);
                    }
                }

                foreach (T t in tempList) {
                    queue.Enqueue(t);
                }

                items.Remove(item);
                return true;
            }
            return false;
        }
        public int RemoveAll(Predicate<T> match) {
            int removedCount = 0;
            List<T> remainingItems = new List<T>();

            while (queue.Count > 0) {
                T item = queue.Dequeue();
                if (match(item)) {
                    removedCount++;
                }
                else {
                    remainingItems.Add(item);
                }
            }

            foreach (T item in remainingItems) {
                queue.Enqueue(item);
            }

            items.RemoveAll(match);

            return removedCount;
        }
        public void Clear() {
            queue.Clear();
            items.Clear();
        }
        public IEnumerator<T> GetEnumerator() {
            return queue.GetEnumerator();
        }
        public T[] ToArray() {
            return items.ToArray();
        }
        public void SortBy<TKey>(Func<T, TKey> keySelector, bool ascending = true) {
            if (ascending)
                items.Sort((a, b) => Comparer<TKey>.Default.Compare(keySelector(a), keySelector(b)));
            else
                items.Sort((a, b) => Comparer<TKey>.Default.Compare(keySelector(b), keySelector(a)));

            SyncQueueFromList();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }




        // Implicit cast to Queue<T>
        public static implicit operator Queue<T>(SerializableQueue<T> serializableQueue) {
            return serializableQueue == null ? null : new Queue<T>(serializableQueue.items);
        }

        // Explicit cast from Queue<T>
        public static explicit operator SerializableQueue<T>(Queue<T> queue) {
            return queue == null ? null : new SerializableQueue<T>(queue);
        }
    }
}