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
        [SerializeField] private List<BaseItemSO> m_InventoryItems;
        
        public void Equip()
        {
            m_CharacterSo.GetCharacterSave().ClearEquipment();
            m_CharacterSo.GetCharacterSave().Save();
            foreach (var armorItem in m_ArmorItems)
            {
                var ins = armorItem.DuplicateUnique() as ArmorItemSO;
                ins.Save();
                m_CharacterSo.GetCharacterSave().EquipItem(ins);
            }
            m_CharacterSo.GetCharacterSave().Save();
        }
        
        public void SetInventory()
        {
            m_CharacterSo.GetCharacterSave().InventoryList = m_InventoryItems.Select((item =>
            {
                var ins = item.DuplicateUnique();
                ins.Save();
                return ins as BaseItemSO;
            })).ToList();
            m_CharacterSo.GetCharacterSave().Save();
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

