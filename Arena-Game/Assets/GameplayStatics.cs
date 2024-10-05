using UnityEngine;

namespace DefaultNamespace
{
    public static class GameplayStatics
    {
        public static bool CheckInternetConnection()
        {
            if(Application.internetReachability == NetworkReachability.NotReachable)
            {
                Debug.Log("Error. Check internet connection!");
                return false;
            }

            return true;
        }
    }
}