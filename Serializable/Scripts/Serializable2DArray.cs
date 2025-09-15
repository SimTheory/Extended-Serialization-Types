using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimTheory {

    [Serializable]
    public class Serializable2DArray<T> {

        [SerializeField] int width;
        [SerializeField] int height;
        [SerializeField] T[] data;




        public Serializable2DArray(int width, int height) {
            this.width = width;
            this.height = height;
            data = new T[width * height];
        }



        public T this[int x, int y] {
            get => data[y * width + x];
            set => data[y * width + x] = value;
        }



        // Ensure data is always initialized after deserialization
        public void OnAfterDeserialize() {
            if (data == null || data.Length != width * height) {
                data = new T[width * height];
            }
        }
        public void OnBeforeSerialize() { }
        public T Find(Func<T, bool> predicate) {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    T value = this[x, y];
                    if (predicate(value)) {
                        return value;
                    }
                }
            }
            return default;
        }
        public bool Exists(Func<T, bool> predicate) {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    if (predicate(this[x, y]))
                        return true;
                }
            }
            return false;
        }
        public Vector2Int IndexOf(T value) {
            var comparer = EqualityComparer<T>.Default;
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    if (comparer.Equals(this[x, y], value))
                        return new Vector2Int(x, y);
                }
            }
            return new Vector2Int(-1, -1); // Not found
        }
        // GetLength method to mimic 2D array behavior
        public int GetLength(int dimension) {
            switch (dimension) {
                case 0: return width;
                case 1: return height;
                default: throw new ArgumentOutOfRangeException(nameof(dimension), "Dimension must be 0 (width) or 1 (height).");
            }
        }





        #region Equals Methods
        public override bool Equals(object obj) => Equals(obj as Serializable2DArray<T>);

        public bool Equals(Serializable2DArray<T> other) {
            if (ReferenceEquals(this, other)) return true;
            if (other == null) return false;
            if (width != other.width || height != other.height) return false;
            if (data == null && other.data == null) return true;
            if (data == null || other.data == null) return false;
            if (data.Length != other.data.Length) return false;

            // Use EqualityComparer for T to support value/reference types
            var comparer = System.Collections.Generic.EqualityComparer<T>.Default;
            for (int i = 0; i < data.Length; i++) {
                if (!comparer.Equals(data[i], other.data[i]))
                    return false;
            }
            return true;
        }

        public override int GetHashCode() {
            unchecked {
                int hash = 17;
                hash = hash * 23 + width.GetHashCode();
                hash = hash * 23 + height.GetHashCode();
                if (data != null) {
                    foreach (var item in data) {
                        hash = hash * 23 + (item == null ? 0 : item.GetHashCode());
                    }
                }
                return hash;
            }
        }

        public static bool operator ==(Serializable2DArray<T> a, Serializable2DArray<T> b) {
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;
            return a.Equals(b);
        }

        public static bool operator !=(Serializable2DArray<T> a, Serializable2DArray<T> b) => !(a == b);
        #endregion
    }
}
