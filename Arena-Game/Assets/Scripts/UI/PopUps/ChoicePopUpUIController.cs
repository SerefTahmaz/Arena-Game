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
    
public class ChoicePopUpUIController : BasePopUp
{
    private cView View => m_PopUpReferenceHelper.View;
    private TMP_Text Text => m_PopUpReferenceHelper.Text;
    private cButton NoButton => m_PopUpReferenceHelper.NoButton;
    private cButton YesButton => m_PopUpReferenceHelper.YesButton;
        
    protected bool waitLock;
    protected bool isSuccessfully;
    
    public static ChoicePopUpUIController Create()
    {
        var ins = GameObject.Instantiate(PrefabList.Get().ChoicePopUpUIController,cUIManager.Instance.transform);
        // ins.Init();
        return ins;
    }

    public static ChoicePopUpUIController CreateFromInstance(ChoicePopUpUIController choicePopUpUIController)
    {
        var ins = Instantiate(choicePopUpUIController,cUIManager.Instance.transform);
        // ins.Init();
        return ins;
    }
    
    public async UniTask<bool> Init(string value)
    {
        NoButton.OnClickEvent.AddListener(HandleNo);
        YesButton.OnClickEvent.AddListener(HandleYes);
        
        View.Deactivate(true);
        View.Activate();
            
        waitLock = true;
    
        Text.text = value;
    
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