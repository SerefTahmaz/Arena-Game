using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using UnityEngine;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private cInputField m_EmailField;
    [SerializeField] private cInputField m_PasswordField;
    [SerializeField] private cButton m_LoginButton;
    
    private IAuthService m_AuthService;
    private bool m_LoginProcessing;

    public void Init(IAuthService authService)
    {
        m_AuthService = authService;
        m_LoginButton.OnClickEvent.AddListener(HandleLogInButtonClicked);
    }

    private void HandleLogInButtonClicked()
    {
        if(m_LoginProcessing) return;
        m_LoginProcessing = true;
        
        LoginUser();
    }

    private async UniTask LoginUser()
    {
        var result = await m_AuthService.SignInWithMailAndPassword(m_EmailField.Text, m_PasswordField.Text);

        switch (result)
        {
            case RequestResult.Failed:
                Debug.Log("Login Failed!");
                break;
            case RequestResult.Success:
                Debug.Log("Login Successful!");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        m_LoginProcessing = false;
    }
}
