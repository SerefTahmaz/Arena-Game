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
        
        private Dictionary<string, CharacterSaveHandler> m_SaveHandlers =
            new Dictionary<string, CharacterSaveHandler>();

        public async UniTask Init()
        {
            m_SaveHandlers.Clear();
            Debug.Log($"Save dict count {m_SaveHandlers.Count}");

            var tasks = new List<UniTask>();
            foreach (var VARIABLE in m_Characters)
            {
                var save = VARIABLE.GetCharacterSave();
                var t = save.Load();
                tasks.Add(t);
            }

            await UniTask.WhenAll(tasks);
        }

        public CharacterSaveHandler GetController(string guid, CharacterSO characterSo)
        {
            if (!m_SaveHandlers.ContainsKey(guid))
            {
                var ins = new CharacterSaveHandler();
                ins.Init(guid,characterSo);
                m_SaveHandlers.Add(guid,ins);
            }

            return m_SaveHandlers[guid];
        }
    }
}