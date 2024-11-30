using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArenaGame.Managers.SaveManager;
using ArenaGame.Utils;
using Cysharp.Threading.Tasks;
using Gameplay;
using UnityEngine;

namespace Authentication
{
    public class CharacterSaveManager : cSingleton<CharacterSaveManager>
    {
        [SerializeField] private List<CharacterSO> m_Characters;
        
        private Dictionary<string, CharacterSaveController> m_SaveControllers =
            new Dictionary<string, CharacterSaveController>();

        public async UniTask Init()
        {
            m_SaveControllers.Clear();
            Debug.Log($"Save dict count {m_SaveControllers.Count}");

            var tasks = new List<UniTask>();
            foreach (var VARIABLE in m_Characters)
            {
                var save = VARIABLE.GetCharacterSave();
                var t = save.Load();
                tasks.Add(t);
            }

            await UniTask.WhenAll(tasks);
        }

        public CharacterSaveController GetController(string guid)
        {
            if (!m_SaveControllers.ContainsKey(guid))
            {
                var ins = new CharacterSaveController();
                ins.Init(guid);
                m_SaveControllers.Add(guid,ins);
            }

            return m_SaveControllers[guid];
        }
    }
}