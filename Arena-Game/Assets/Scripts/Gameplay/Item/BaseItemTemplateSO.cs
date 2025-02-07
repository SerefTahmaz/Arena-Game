using UnityEngine;

namespace Item
{
    public class BaseItemTemplateSO : SerializableScriptableObject
    {
        [SerializeField] private Sprite m_ItemSprite;
        
        public Sprite ItemSprite => m_ItemSprite;
    }

    public enum ItemType
    {
        Weapon,
        Armor,
        Consumable
    }
}