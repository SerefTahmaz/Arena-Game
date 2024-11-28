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
    
public class ChoicePopUpUIController : MonoBehaviour
{
    [SerializeField] private cView m_View;
    [SerializeField] private TMP_Text m_Text;
    [SerializeField] private cButton m_NoButton;
    [SerializeField] private cButton m_YesButton;
        
    protected bool waitLock;
    protected bool isSuccessfully;
    
    public static ChoicePopUpUIController Create()
    {
        var ins = GameObject.Instantiate(PrefabList.Get().ChoicePopUpUIController,cUIManager.Instance.transform);
        // ins.Init();
        return ins;
    }
    
    public async UniTask<bool> Init(string value)
    {
        m_NoButton.OnClickEvent.AddListener(HandleNo);
        m_YesButton.OnClickEvent.AddListener(HandleYes);
        
        m_View.Deactivate(true);
        m_View.Activate();
            
        waitLock = true;
    
        m_Text.text = value;
    
        await UniTask.WaitWhile((() => waitLock));
    
        return isSuccessfully;
    }
    
    public virtual void HandleYes()
    {
        isSuccessfully = true;
        waitLock = false;
        gameObject.SetActive(false);
    }
    
    public virtual void HandleNo()
    {
        isSuccessfully = false;
        waitLock = false;
        gameObject.SetActive(false);
    }
}