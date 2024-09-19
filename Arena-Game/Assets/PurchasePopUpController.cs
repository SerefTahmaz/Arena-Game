using System;
using System.Collections;
using System.Collections.Generic;
using _Main.Scripts;
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

    private void Awake()
    {
        m_NoButton.OnClickEvent.AddListener(HandleNo);
        m_YesButton.OnClickEvent.AddListener(HandleYes);
    }

    private bool waitLock;
    private bool isSuccessfully;

    public async UniTask<bool> Init(string itemToPurchaseName, string value)
    {
        m_View.Deactivate(true);
        m_View.Activate();
        
        waitLock = true;
        
        m_PurchaseText.text =
            $"Purchase {itemToPurchaseName.ColorHtmlString(m_Color)} for {value.ColorHtmlString(m_Color)}";

        await UniTask.WaitWhile((() => waitLock));

        return isSuccessfully;
    }

    public void HandleYes()
    {
        isSuccessfully = true;
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
