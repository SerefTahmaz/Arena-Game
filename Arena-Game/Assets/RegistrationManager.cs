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
        var result = await m_AuthService.CreateUserWithMailAndPassword(m_EmailField.Text, m_PasswordField.Text);

        switch (result)
        {
            case RequestResult.Failed:
                Debug.Log("Register Failed!");
                break;
            case RequestResult.Success:
                Debug.Log("Register Successful!");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        m_RegisterProcessing = false;
    }
}
