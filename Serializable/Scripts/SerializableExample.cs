using UnityEngine;

namespace SimTheory {
    [ExecuteInEditMode]
    /// <summary>
    /// Attach to a GameObject to see examples of using the serializable collections in the inspector.
    /// </summary>
    public class SerializableExample : MonoBehaviour { 

        public Serializable2DArray<int> array2D = new Serializable2DArray<int>(3, 3);
        [Space]
        public SerializableDictionary<string, float> dictionary = new SerializableDictionary<string, float>();
        [Space]
        public SerializableHashSet<float> hashSet = new SerializableHashSet<float>();
        [Space]
        public SerializableQueue<Vector2> vector2s = new SerializableQueue<Vector2>();
        [Space]
        public SerializableDateTime dateTime = new SerializableDateTime(System.DateTime.Now);


    }
}
