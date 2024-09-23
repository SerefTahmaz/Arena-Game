using Item;
using Item.Scripts;
using UnityEngine;

namespace Gameplay.Item
{
    [CreateAssetMenu(fileName = "Armor Item", menuName = "Game/Item/Template/Template Armor Item", order = 0)]
    public class ArmorItemTemplate : BaseItemTemplateSO
    {
        [SerializeField] private ArmorController m_ArmorPrefab;
        [SerializeField] private ArmorType m_ArmorType;
        [SerializeField] private Texture m_BodyMask;
        [SerializeField] private bool m_HideHair;

        public ArmorController ArmorPrefab => m_ArmorPrefab;
        public ArmorType ArmorType => m_ArmorType;
        public Texture BodyMask => m_BodyMask;
        public bool HideHair => m_HideHair;
    }
 
    public enum ArmorType
    {
        Helm,
        Chest,
        Gauntlets,
        Legging
    }
}