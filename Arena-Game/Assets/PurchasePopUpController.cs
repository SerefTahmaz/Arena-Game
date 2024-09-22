using System;
using System.Collections;
using System.Collections.Generic;
using _Main.Scripts;
using ArenaGame.Currency;
using ArenaGame.UI;
using ArenaGame.Utils;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PurchasePopUpController : MonoBehaviour, IPurchasePopUpController
{
    [SerializeField] private cView m_View;
    [SerializeField] private TMP_Text m_PurchaseText;
    [SerializeField] private Color m_Color;
    [SerializeField] private cButton m_NoButton;
    [SerializeField] private cButton m_YesButton;

    private int m_Amount;

    private void Awake()
    {
        m_NoButton.OnClickEvent.AddListener(HandleNo);
        m_YesButton.OnClickEvent.AddListener(HandleYes);
    }

    private bool waitLock;
    private bool isSuccessfully;

    public async UniTask<bool> Init(string itemToPurchaseName, int value)
    {
        m_Amount = value;
        
        m_View.Deactivate(true);
        m_View.Activate();
        
        waitLock = true;
        
        m_PurchaseText.text =
            $"Purchase {itemToPurchaseName.ColorHtmlString(m_Color)} for {m_Amount.ToString().ColorHtmlString(m_Color)}";

        await UniTask.WaitWhile((() => waitLock));

        return isSuccessfully;
    }

    public void HandleYes()
    {
        if (CurrencyManager.HasEnoughCurrency(m_Amount))
        {
            isSuccessfully = true;
        }
        else
        {
            var infoPopUp=GlobalFactory.InfoPopUpFactory.Create();
            infoPopUp.Init("Not enough currency!!!");
            isSuccessfully = false;
        }
        waitLock = false;
        gameObject.SetActive(false);
    }

    public void HandleNo()
    {
        isSuccessfully = false;
        waitLock = false;
        gameObject.SetActive(false);
    }
}
