using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArenaGame.UI.MenuInventory
{
    public class ArmorMenuInventoryItemController : MenuInventoryItemController
    {
        [SerializeField] private Image m_Image;
        [SerializeField] private TMP_Text m_LevelText;
        [SerializeField] private TMP_Text m_LevelIncrementText;
        [SerializeField] private Image m_LevelIncrementFill;
        

        public ArmorItemSO itemTemplate;

        public void Init(ArmorItemSO itemTemplate, bool isWearing, IMenuInventoryItemHandler menuInventoryItemHandler)
        {
            base.Init(menuInventoryItemHandler);
            this.itemTemplate = itemTemplate;
            m_Image.sprite = itemTemplate.ItemTemplate.ItemSprite;

            m_LevelText.text = $"LVL{this.itemTemplate.Level}";
            m_LevelIncrementText.text = $"{this.itemTemplate.NextLevelIncrement}/6";
            m_LevelIncrementFill.fillAmount = ((float)this.itemTemplate.NextLevelIncrement / 6);

            if (isWearing)
            {
                m_EquipedLayer.SetActive(true);
            }
        }
    }
}