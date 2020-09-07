using UnityEngine;

namespace BurningBlueFox.Heated.Core
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    var gameObject = new GameObject(typeof(T).Name);
                    DontDestroyOnLoad(gameObject);
                    instance = gameObject.AddComponent<T>();
                }
                return instance;
            }
        }
    }
}
