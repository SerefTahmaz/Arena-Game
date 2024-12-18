using ArenaGame.Managers.SaveManager;
using ArenaGame.Utils;
using Authentication;
using DefaultNamespace;
using UnityEngine;

namespace ArenaGame
{
    public class ProfileGenerator : MonoBehaviour
    {
        public static PlayerDataModel LastGeneratedRandomProfile { get; set; }
        
        public static PlayerDataModel GetRandomProfile()
        {
            var randomImage = Resources.Load<Texture2D>($"MatchMaking/PPs/PP ({Random.Range(1, 573)})");
            var randomName = Resources.Load<TextAsset>("MatchMaking/RandomProfileNames");
            var randomNames = randomName.text.Split("\n");
            var randomExp = UserSaveHandler.SaveData.m_ExperiencePoint;
            randomExp = Mathf.FloorToInt(Random.Range(0.6f, 2.0f) * randomExp);
            if (randomExp > 20)
            {
                var mod = randomExp % 20;
                randomExp -= mod;
            }
            LastGeneratedRandomProfile = new PlayerDataModel(randomNames.RandomItem(), randomImage, randomExp);
            return LastGeneratedRandomProfile;
        } 

        public static PlayerDataModel GetPlayerProfile()
        {
            UserSaveHandler.Load();
            var savaData = UserSaveHandler.SaveData;
            return new PlayerDataModel(savaData, UserSaveHandler.GetProfileImage());
        }

        public static void SaveProfileImage(Texture2D texture)
        {
            UserSaveHandler.SaveProfileImage(texture);
        }

        public static void SaveProfileName(string newName)
        {
            UserSaveHandler.Load();
            var savaData = UserSaveHandler.SaveData;
            savaData.m_Username = newName;
            UserSaveHandler.Save();
        }
    }

    public class PlayerDataModel
    {
        public PlayerDataModel(string name, Texture2D profilePicture, int expPoint)
        {
            Name = name;
            ProfilePicture = profilePicture;
            ExpPoint = expPoint;
        }

        public PlayerDataModel(UserData saveData, Texture2D profilePicture)
        {
            Name = saveData.m_Username;
            ProfilePicture = profilePicture;
            ProfilePictureURL = saveData.m_ProfileImageUrl;
            ExpPoint = saveData.m_ExperiencePoint;
            WinsCount = saveData.m_WinsCount;
        }

        public string Name { get; set; }
        public Texture2D ProfilePicture { get; set; }
        public string ProfilePictureURL { get; set; }
        public int ExpPoint { get; set; }
        public int Currency => GameplayStatics.GetPlayerCharacterSO().GetCharacterSave().Currency;
        public int WinsCount { get; set; }
    }
}