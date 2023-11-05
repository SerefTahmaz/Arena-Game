using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DemoBlast.Managers.SaveManager
{
    public class cSaveManager : MonoBehaviour, ISaveManager
    {
        private bool m_Loaded;
    
        public cSaveData SaveData
        {
            get
            {
                if (!m_Loaded)
                {
                    cSaveDataHandler.Load();
                    m_Loaded = true;
                }
                return cSaveDataHandler.SaveData;
            }
            set => cSaveDataHandler.SaveData = value;
        }

        private void Awake()
        {
            SaveLoop().Forget();
        }

        private async UniTaskVoid SaveLoop()
        {
            while (true)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(20));
                cSaveDataHandler.Save();
            }
        }

        public void Save()
        {
            cSaveDataHandler.Save();
        }

        public void Load()
        {
            cSaveDataHandler.Load();
        }
    }
}