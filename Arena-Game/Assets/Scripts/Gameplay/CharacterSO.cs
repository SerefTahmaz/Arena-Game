using System;
using System.Collections.Generic;
using System.Linq;
using ArenaGame.Managers.SaveManager;
using Authentication;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Firebase.Database;
using Gameplay.Item;
using Item;
using Newtonsoft.Json;
using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "Character", menuName = "Game/CharacterSSS", order = 0)]
    public class CharacterSO : SerializableScriptableObject
    { 
        [SerializeField] private int m_StartHealth;

        public int StartHealth => m_StartHealth;

        public CharacterSaveHandler GetCharacterSave()
        {
            if (!CharacterSaveManager.Instance) return null;
            return CharacterSaveManager.Instance.GetController(Guid.ToHexString(),this);
        }
    }
    
   
}