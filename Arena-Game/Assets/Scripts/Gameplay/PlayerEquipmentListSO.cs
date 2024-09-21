using System.Collections.Generic;
using System.Linq;
using ArenaGame.Managers.SaveManager;
using DefaultNamespace.ArenaGame.Managers.SaveManager;
using Gameplay;
using Gameplay.Item;
using Item;
using UnityEngine;

    
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "Equipment", menuName = "Game/EquipmentListSO", order = 0)]
    public class PlayerEquipmentListSO : ScriptableObject
    {
        [SerializeField] private CharacterSO m_CharacterSo;
        [SerializeField] private List<ArmorItem> m_ArmorItems;
        
        public void Equip()
        {
            m_CharacterSo.Load();
            m_CharacterSo.ClearEquipment();
            m_CharacterSo.Save();
            foreach (var armorItem in m_ArmorItems)
            {
                m_CharacterSo.EquipItem(armorItem);
            }
            m_CharacterSo.Save();
        }
        
        public void SetInventory()
        {
            m_CharacterSo.Load();
            m_CharacterSo.InventoryList = m_ArmorItems.Select((item => item as BaseItemSO)).ToList();
            m_CharacterSo.Save();
        }
    }
    


#if UNITY_EDITOR
    [CustomEditor(typeof(PlayerEquipmentListSO))]
    public class PlayerEquipmentListSOEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Equip"))
            {
                (target as PlayerEquipmentListSO).Equip();
            }
            if (GUILayout.Button("SetInventory"))
            {
                (target as PlayerEquipmentListSO).SetInventory();
            }
        }
    }
#endif
}

