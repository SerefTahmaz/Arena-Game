using Gameplay.Farming;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArenaGame.UI.MenuInventory
{
    public class ConsumableInventoryItemController : MenuInventoryItemController
    {
        [SerializeField] private Image m_Image;
        
        public BaseItemSO itemTemplate;

        public void Init(BaseItemSO itemTemplate, IMenuInventoryItemHandler menuInventoryItemHandler)
        {
            base.Init(menuInventoryItemHandler);
            this.itemTemplate = itemTemplate;
            m_Image.sprite = itemTemplate.ItemSprite;
        }
    }
}