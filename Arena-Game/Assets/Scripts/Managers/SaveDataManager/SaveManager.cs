using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace ArenaGame.Managers.SaveManager
{
    public class SaveManager : MonoBehaviour, ISaveManager
    {
        private bool m_Loaded;
    
        public SaveData SaveData
        {
            get
            {
                if (!m_Loaded)
                {
                    SaveGameHandler.Load();
                    m_Loaded = true;
                }
                return SaveGameHandler.SaveData;
            }
            set => SaveGameHandler.SaveData = value;
        }

        private void Awake()
        {
            SaveGameHandler.Load();
            
            SaveLoop().Forget();
            DOVirtual.DelayedCall(2, () =>
            {
                SaveGameHandler.Save();
            });
        }

        private async UniTaskVoid SaveLoop()
        {
            while (true)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(20));
                SaveGameHandler.Save();
            }
        }

        public void Save()
        {
            SaveGameHandler.Save();
        }

        public void Load()
        {
            SaveGameHandler.Load();
        }
    }
}