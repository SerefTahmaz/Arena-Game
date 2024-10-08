using Gameplay.Farming;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ArenaGame.UI.MenuInventory
{
    public class ConsumableInventoryItemController : MenuInventoryItemController
    {
        [SerializeField] private Image m_Image;
        
        public BaseItemSO itemSO;

        public void Init(BaseItemSO itemTemplate, IMenuInventoryItemHandler menuInventoryItemHandler)
        {
            base.Init(menuInventoryItemHandler);
            this.itemSO = itemTemplate;
            m_Image.sprite = itemTemplate.ItemSprite;
        }
    }
}