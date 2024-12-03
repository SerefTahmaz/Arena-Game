using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

public class RegistrationManager : MonoBehaviour
{
    [SerializeField] private cInputField m_EmailField;
    [SerializeField] private cInputField m_PasswordField;
    [SerializeField] private cInputField m_RepeatPasswordField;
     
    [SerializeField] private cButton m_RegisterButton;
    [SerializeField] private cMenuNode m_MenuNode;
    [SerializeField] private GameObject m_PasswordNotMatchingWarning;
    [SerializeField] private GameObject m_EmailEmptyWarning;
    [SerializeField] private GameObject m_PasswordEmptyWarning;

    private bool m_RegisterProcessing;
    private bool m_IsPasswordMatching;
    
    private IAuthService m_AuthService;
    
    public Action OnLoggedInUser { get; set; }

    public void Init(IAuthService authService)
    {
        m_AuthService = authService;
        m_RegisterButton.OnClickEvent.AddListener(HandleRegisterButtonClicked);

        m_PasswordField.OnValueChanged.AddListener(CheckPasswordMatching);
        m_RepeatPasswordField.OnValueChanged.AddListener(CheckPasswordMatching);
        m_EmailField.OnValueChanged.AddListener(HandleEmailEditing);
        m_PasswordField.OnValueChanged.AddListener(HandlePasswordEditing);
    }

    private void HandlePasswordEditing(string arg0)
    {
        if(arg0.Length > 0) m_PasswordEmptyWarning.SetActive(false);
    }

    private void HandleEmailEditing(string arg0)
    {
        if(arg0.Length > 0) m_EmailEmptyWarning.SetActive(false);
    }

    private void CheckPasswordMatching(string arg0)
    {
        m_IsPasswordMatching = m_PasswordField.Text == m_RepeatPasswordField.Text;
        m_PasswordNotMatchingWarning.SetActive(!m_IsPasswordMatching);
    }

    private void HandleRegisterButtonClicked()
    {
        RegisterUser();
    }

    private async UniTask RegisterUser()
    {
        if (!m_IsPasswordMatching)
        {
            Debug.Log("Passwords matching");
            return;
        } 
        
        if (String.IsNullOrEmpty(m_EmailField.Text))
        {
            m_EmailEmptyWarning.SetActive(true);
            Debug.Log("Email empty!");
            return;
        }
        if (String.IsNullOrEmpty(m_PasswordField.Text))
        {
            m_PasswordEmptyWarning.SetActive(true);
            Debug.Log("Password empty!");
            return;
        }
        
        m_RegisterButton.DeActivate();
        MiniLoadingScreen.Instance.ShowPage(this);
        
        var result = await m_AuthService.CreateUserWithMailAndPassword(m_EmailField.Text, m_PasswordField.Text);

        switch (result)
        {
            case RequestResult.Failed:
                Debug.Log("Register Failed!");
                break;
            case RequestResult.Success:
                Debug.Log("Register Successful!");
                OnLoggedInUser?.Invoke();
                AuthManager.Instance.AuthenticateUserAndConfigureUI();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        MiniLoadingScreen.Instance.HidePage(this);
        m_RegisterButton.Activate();
    }
}
