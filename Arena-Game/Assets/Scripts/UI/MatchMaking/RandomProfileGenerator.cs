using DemoBlast.Utils;
using UnityEngine;

namespace ArenaGame
{
    public class RandomProfileGenerator : MonoBehaviour
    {
        public static PlayerDataModel GenerateProfile()
        {
            var randomImage = Resources.Load<Texture2D>($"MatchMaking/PPs/PP ({Random.Range(1, 573)})");
            var randomName = Resources.Load<TextAsset>("MatchMaking/RandomProfileNames");
            var randomNames = randomName.text.Split("\n");
            return new PlayerDataModel(randomNames.RandomItem(), randomImage, Random.Range(50, 250));
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

        public string Name { get; set; }
        public Texture2D ProfilePicture { get; set; }
        public int ExpPoint { get; set; }
        public int Currency { get; set; }
    }
}