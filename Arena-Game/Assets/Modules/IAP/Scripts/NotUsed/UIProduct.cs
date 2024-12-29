using System;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class UIProduct : MonoBehaviour
{
    [SerializeField] private TMP_Text m_NameText;
    [SerializeField] private TMP_Text m_DescriptionText;
    [SerializeField] private Image m_Icon;
    [SerializeField] private TMP_Text m_PriceText;
    [SerializeField] private Button m_PurchaseButton;

    public delegate void PurchaseEvent(Product model, Action onComplete);
    public event PurchaseEvent OnPurchase;
    
    private Product m_Model;

    public void Setup(Product product)
    {
        m_Model = product;
        m_NameText.SetText(product.metadata.localizedTitle);
        m_DescriptionText.SetText(product.metadata.localizedDescription);
        m_PriceText.SetText($"{product.metadata.localizedPriceString}" +$"{product.metadata.isoCurrencyCode}");
        Texture2D texture = StoreIconProvider.GetIcon(product.definition.id);
        if (texture != null)
        {
            var sprite = Sprite.Create(texture,
                new Rect(0, 0, texture.width, texture.height),
                Vector2.one / 2);

            m_Icon.sprite = sprite;
        }
        else
        {
            Debug.LogError($"No sprite found for {product.definition.id}! ");
        }
    }

    public void Purchase()
    {
        m_PurchaseButton.enabled = false;
        OnPurchase?.Invoke(m_Model,HandlePurchaseComplete);
    }

    private void HandlePurchaseComplete()
    {
        m_PurchaseButton.enabled = true;
    }
}