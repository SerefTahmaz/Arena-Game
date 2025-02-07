using Unity.Netcode;
using UnityEngine;

namespace ArenaGame.Utils
{
    public class cSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static object _lock = new object();
        private static T _instance;

        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null) _instance = (T) FindObjectOfType(typeof(T));

                    return _instance;
                }
            }
        }
    }
    
    public class BaseSingleton<T> : System.Object where T : new()
    {
        private static object _lock = new object();
        private static T _instance;

        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null) _instance = (T)new T();

                    return _instance;
                }
            }
        }
    }
}

namespace ArenaGame.Utils
{
    public class cNetworkSingleton<T> : NetworkBehaviour where T : NetworkBehaviour
    {
        private static object _lock = new object();
        private static T _instance;

        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null) _instance = (T) FindObjectOfType(typeof(T));

                    return _instance;
                }
            }
        }
    }
}