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
            UserSaveHandler.Load();
            return UserSaveHandler.SaveData.m_ExperiencePoint;
        }
        
        public static void GainExperience(int amount)
        {
            UserSaveHandler.Load();
            var savaData = UserSaveHandler.SaveData;
            savaData.m_ExperiencePoint += amount;
            UserSaveHandler.Save();
        }

        public static void LoseExperience(int amount)
        {
            UserSaveHandler.Load();
            var savaData = UserSaveHandler.SaveData;
            savaData.m_ExperiencePoint -= amount;

            if (savaData.m_ExperiencePoint < 0)
            {
                savaData.m_ExperiencePoint = 0;
                Debug.Log("Currency cant be less than zero!!!!");
            }
            UserSaveHandler.Save();
        }

        public static bool HasEnoughExperience(int amount)
        {
            return Experience() >= amount;
        }
    }
}