using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cSetGameMode : MonoBehaviour
{
    [SerializeField] private eGameMode m_GameMode;

    public void OnClick()
    {
        cGameManager.Instance.CurrentGameMode = m_GameMode;
    }
}
