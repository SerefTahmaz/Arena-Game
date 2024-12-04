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
            LastGeneratedRandomProfile = new PlayerDataModel(randomNames.RandomItem(), randomImage, Random.Range(50, 250));
            return LastGeneratedRandomProfile;
        }

        public static PlayerDataModel GetPlayerProfile()
        {
            SaveGameHandler.Load();
            var savaData = SaveGameHandler.SaveData;
            return new PlayerDataModel(savaData, SaveGameHandler.GetProfileImage());
        }

        public static void SaveProfileImage(Texture2D texture)
        {
            SaveGameHandler.SaveProfileImage(texture);
        }

        public static void SaveProfileName(string newName)
        {
            SaveGameHandler.Load();
            var savaData = SaveGameHandler.SaveData;
            savaData.m_Username = newName;
            SaveGameHandler.Save();
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

        public PlayerDataModel(User saveData, Texture2D profilePicture)
        {
            Name = saveData.m_Username;
            ProfilePicture = profilePicture;
            ExpPoint = saveData.m_ExperiencePoint;
            WinsCount = saveData.m_WinsCount;
        }

        public string Name { get; set; }
        public Texture2D ProfilePicture { get; set; }
        public int ExpPoint { get; set; }
        public int Currency => GameplayStatics.GetPlayerCharacterSO().GetCharacterSave().Currency;
        public int WinsCount { get; set; }
    }
}