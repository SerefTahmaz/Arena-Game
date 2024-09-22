using System;
using ArenaGame.Managers.SaveManager;
using ArenaGame.Utils;
using UnityEngine;

namespace ArenaGame.Experience
{
    public class ExperienceManager
    {
        public static int Experience()
        {
            SaveGameHandler.Load();
            return SaveGameHandler.SaveData.m_ExperiencePoint;
        }
        
        public static void GainExperience(int amount)
        {
            SaveGameHandler.Load();
            var savaData = SaveGameHandler.SaveData;
            savaData.m_ExperiencePoint += amount;
            SaveGameHandler.Save();
        }

        public static void LoseExperience(int amount)
        {
            SaveGameHandler.Load();
            var savaData = SaveGameHandler.SaveData;
            savaData.m_ExperiencePoint -= amount;

            if (savaData.m_ExperiencePoint < 0)
            {
                Debug.Log("Currency cant be less than zero!!!!");
            }
            SaveGameHandler.Save();
        }

        public static bool HasEnoughExperience(int amount)
        {
            return Experience() >= amount;
        }
    }
}