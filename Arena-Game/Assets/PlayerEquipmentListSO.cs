using System.Collections.Generic;
using System.Linq;
using ArenaGame.Managers.SaveManager;
using DefaultNamespace.ArenaGame.Managers.SaveManager;
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
            m_CharacterSo.EquipmentList = m_ArmorItems;
            m_CharacterSo.Save();
        }
    }
    


#if UNITY_EDITOR
    [CustomEditor(typeof(PlayerEquipmentListSO))]
    public class TemplateEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Click"))
            {
                (target as PlayerEquipmentListSO).Equip();
            }
        }
    }
#endif
}

