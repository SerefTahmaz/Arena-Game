using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthManager : MonoBehaviour
{
    [SerializeField] private FirebaseAuthManager m_FirebaseAuthManager;
    [SerializeField] private LoginManager m_LoginManager;

    private void Awake()
    {
        m_FirebaseAuthManager.Init();
        m_LoginManager.Init(m_FirebaseAuthManager);
    }
}
