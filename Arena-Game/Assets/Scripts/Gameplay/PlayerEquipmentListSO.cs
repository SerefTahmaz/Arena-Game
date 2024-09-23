using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private List<ArmorItemSO> m_ArmorItems;
        
        public void Equip()
        {
            m_CharacterSo.Load();
            m_CharacterSo.ClearEquipment();
            m_CharacterSo.Save();
            foreach (var armorItem in m_ArmorItems)
            {
                var ins = armorItem.DuplicateUnique() as ArmorItemSO;
                ins.Save();
                m_CharacterSo.EquipItem(ins);
            }
            m_CharacterSo.Save();
        }
        
        public void SetInventory()
        {
            m_CharacterSo.Load();
            m_CharacterSo.InventoryList = m_ArmorItems.Select((item =>
            {
                var ins = item.DuplicateUnique();
                ins.Save();
                return ins as BaseItemSO;
            })).ToList();
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

