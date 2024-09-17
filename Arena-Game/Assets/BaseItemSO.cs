using UnityEngine;

namespace Item
{
    public class BaseItemSO : SerializableScriptableObject
    {
        [SerializeField] private string m_ItemName;
        [SerializeField] private ItemType m_ItemType;
        [SerializeField] private Sprite m_ItemSprite;
    }

    public enum ItemType
    {
        Weapon,
        Armor,
        Consumable
    }
}