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
    [SerializeField] private cButton m_DontHaveAccountButton;
    [SerializeField] private cMenuNode m_MenuNode;
    
    private IAuthService m_AuthService;
    private bool m_LoginProcessing;
    
    public Action OnLoggedInUser { get; set; }

    public void Init(IAuthService authService)
    {
        m_AuthService = authService;
        m_LoginButton.OnClickEvent.AddListener(HandleLogInButtonClicked);
        m_DontHaveAccountButton.OnClickEvent.AddListener(HandleDontHaveAccountButtonCLicked);
    }

    private void HandleDontHaveAccountButtonCLicked()
    {
        
    }

    private void HandleLogInButtonClicked()
    {
        if(m_LoginProcessing) return;
        m_LoginProcessing = true;
        
        LoginUser();
    }

    private async UniTask LoginUser()
    {
        MiniLoadingScreen.Instance.ShowPage(this);
        var result = await m_AuthService.SignInWithMailAndPassword(m_EmailField.Text, m_PasswordField.Text);

        switch (result)
        {
            case RequestResult.Failed:
                Debug.Log("Login Failed!");
                break;
            case RequestResult.Success:
                Debug.Log("Login Successful!");
                OnLoggedInUser?.Invoke();
                AuthManager.Instance.AuthenticateUserAndConfigureUI();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        MiniLoadingScreen.Instance.HidePage(this);
        m_LoginProcessing = false;
    }

    public void ActivateUI()
    {
        m_MenuNode.Activate();
    }
}
