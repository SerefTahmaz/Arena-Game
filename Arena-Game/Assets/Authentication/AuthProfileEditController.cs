using System;
using ArenaGame;
using ArenaGame.Managers.SaveManager;
using ArenaGame.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class AuthProfileEditController : EditProfileController
{
    [SerializeField] private cButton m_DismissButton;
    
    private bool m_Dismissed;

    protected override void Awake()
    {
        base.Awake();
        m_DismissButton.OnClickEvent.AddListener(HandleOnDismissed);
    }

    public async UniTask Init()
    {
        m_MenuNode.Activate();
        
        m_Dismissed = false;
        await UniTask.WaitUntil((() => m_Dismissed));
        
        m_MenuNode.Deactivate(true);
    }
  
    private void HandleOnDismissed()
    {
        m_Dismissed = true;
    }
}