using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace DemoBlast.UI.Currency
{
    public class cCurrencyBar : MonoBehaviour
    {
        [SerializeField] private cCurrencyBarScreen m_CurrencyBarScreen;
        [SerializeField] private TextMeshProUGUI m_CurrencyText;
        
        private RectTransform m_CurrencyTextTransform;
    
        void Start()
        {
            m_CurrencyBarScreen.RegisterBar(this);
            m_CurrencyTextTransform = m_CurrencyText.rectTransform;
            Refresh(m_CurrencyBarScreen.CurrentCurrencyAmount);
        }
        public void Refresh(int currency)
        {
            m_CurrencyText.SetText($"{currency}");
            m_CurrencyTextTransform.DOKill();
            m_CurrencyTextTransform.transform.localScale = Vector3.one;
            m_CurrencyTextTransform.DOScale(.25f, .075f).SetLoops(2, LoopType.Yoyo).SetRelative(true);
        }
    }
}