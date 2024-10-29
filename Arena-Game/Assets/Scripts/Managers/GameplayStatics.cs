using System.Linq;
using Gameplay;
using Gameplay.Character;
using UnityEngine;

namespace DefaultNamespace
{
    public static class GameplayStatics
    {
        public static Transform OwnerPlayer { get; set; }
        
        public static bool CheckInternetConnection()
        {
            if(Application.internetReachability == NetworkReachability.NotReachable)
            {
                Debug.Log("Error. Check internet connection!");
                return false;
            }

            return true;
        }

        public static CharacterSO GetPlayerCharacterSO()
        {
            var playerCharacter = Resources.Load<CharacterSO>("Characters/Player");
            return playerCharacter;
        }
        public static void SetPlayerVisibility(bool value)
        {
            InputManager.Instance.SetInput(value);
            CameraManager.Instance.SetInput(value);
            
            Debug.Log($"Owner player {OwnerPlayer!=null}");
            if (OwnerPlayer)
            {
                OwnerPlayer.GetComponent<HumanCharacter>().SetVisibility(value);
            }
        }
    }
}