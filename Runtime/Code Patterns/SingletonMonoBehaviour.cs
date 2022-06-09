using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Code_Patterns
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : Component
    {
        public static T Instance { get; private set; }

        protected abstract void Initialize();

        public virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this as T;
                transform.SetParent(null);
                DontDestroyOnLoad(this);
                Initialize(); 
            }
        }
    }
}