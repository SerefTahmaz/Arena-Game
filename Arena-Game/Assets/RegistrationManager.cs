using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using UnityEngine;

public class RegistrationManager : MonoBehaviour
{
    [SerializeField] private cInputField m_EmailField;
    [SerializeField] private cInputField m_PasswordField;
    [SerializeField] private cInputField m_UserNameField;
    [SerializeField] private ImagePicker m_ImagePicker;
    [SerializeField] private cButton m_RegisterButton;
    [SerializeField] private cMenuNode m_MenuNode;

    private bool m_RegisterProcessing;
    
    private IAuthService m_AuthService;

    public void Init(IAuthService authService)
    {
        m_AuthService = authService;
        m_RegisterButton.OnClickEvent.AddListener(HandleRegisterButtonClicked);
    }

    private void HandleRegisterButtonClicked()
    {
        if(m_RegisterProcessing) return;
        m_RegisterProcessing = true;
        
        RegisterUser();
    }

    private async UniTask RegisterUser()
    {
        if (m_ImagePicker.Image == null)
        {
            Debug.Log("Please select a profile image!");
            return;
        }
        if (String.IsNullOrEmpty(m_EmailField.Text))
        {
            Debug.Log("Please enter a email");
            return;
        }
        if (String.IsNullOrEmpty(m_PasswordField.Text))
        {
            Debug.Log("Please enter a password");
            return;
        }
        if (String.IsNullOrEmpty(m_UserNameField.Text))
        {
            Debug.Log("Please enter a username");
            return;
        }
        
        var credentials = new AuthCredentials(m_EmailField.Text, m_PasswordField.Text, m_UserNameField.Text, m_ImagePicker.Image);
        
        var result = await m_AuthService.RegisterUser(credentials);

        switch (result)
        {
            case RequestResult.Failed:
                Debug.Log("Register Failed!");
                break;
            case RequestResult.Success:
                Debug.Log("Register Successful!");
                m_MenuNode.Deactivate();
                AuthManager.Instance.AuthenticateUserAndConfigureUI();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        m_RegisterProcessing = false;
    }
}
