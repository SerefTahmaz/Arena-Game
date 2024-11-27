using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthManager : MonoBehaviour
{
    [SerializeField] private FirebaseAuthManager m_FirebaseAuthManager;
    [SerializeField] private LoginManager m_LoginManager;
    [SerializeField] private RegistrationManager m_RegistrationManager;

    private void Awake()
    {
        m_FirebaseAuthManager.Init();
        m_LoginManager.Init(m_FirebaseAuthManager);
        m_RegistrationManager.Init(m_FirebaseAuthManager);
    }
}
