using Item;
using UnityEngine;

namespace Gameplay.Item
{
    [CreateAssetMenu(fileName = "Armor Item", menuName = "Item/Armor Item", order = 0)]
    public class ArmorItem : BaseItemSO
    {
        [SerializeField] private ArmorController m_ArmorPrefab;
        [SerializeField] private ArmorType m_ArmorType;

        public ArmorController ArmorPrefab => m_ArmorPrefab;
        public ArmorType ArmorType => m_ArmorType;
    }

    public enum ArmorType
    {
        Helm,
        Chest,
        Gauntlets,
        Legging
    }
}