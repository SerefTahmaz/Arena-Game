using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class cMainMenuButton : MonoBehaviour
{
    public void OnClick()
    {
        cGameManager.Instance.MainMenuButton();
    }
}