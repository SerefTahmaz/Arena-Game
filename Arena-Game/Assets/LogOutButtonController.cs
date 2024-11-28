using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using UnityEngine;

public class LogOutButtonController : MonoBehaviour
{
    [SerializeField] private cButton m_Button;

    public Action OnLogOut { get; set; }
    
    // Start is called before the first frame update
    void Start()
    {
        m_Button.OnClickEvent.AddListener(HandleButtonClicked);
    }

    private void HandleButtonClicked()
    {
        m_Button.DeActivate();
        CheckAndLogout();
    }

    private async UniTask CheckAndLogout()
    {
        var insChoice = ChoicePopUpUIController.Create();

        var isSuccessful = await insChoice.Init("Are you sure you want to log out?");
 
        if (isSuccessful)
        {
            AuthManager.Instance.SignOut();
            OnLogOut?.Invoke();
        }
        
        m_Button.Activate();
    }
}
