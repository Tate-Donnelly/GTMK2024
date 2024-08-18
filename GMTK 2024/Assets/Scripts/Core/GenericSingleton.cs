using UnityEngine;

namespace Core
{
    /// <summary>
    /// Generically typed abstraction for singletons.
    /// </summary>
    public abstract class GenericSingleton<T> : MonoBehaviour where T : GenericSingleton<T>
    {
        // Singleton
        public static T Instance { get; private set; }


        protected virtual void Awake()
        {
            // Singleton
            if (Instance == null) {
                Instance = (T) this;
            } else {
                DestroyImmediate(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            if (Instance == this) {
                Instance = null;
            }
        }
    }
}
