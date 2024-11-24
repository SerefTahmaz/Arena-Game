using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class MobileFightUIHelper : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_FightUIs;
 
    // Update is called once per frame
    void Update()
    {
        var ownerPlayer = cGameManager.Instance.m_OwnerPlayer;

        if (ownerPlayer != null && (ownerPlayer.CharacterStateMachine.IsLeftSwordDrawn ||
            ownerPlayer.CharacterStateMachine.IsRightSwordDrawn))
        {
            SetActiveUIs(true);
        }
        else
        {
            SetActiveUIs(false);
        }
    }

    private void SetActiveUIs(bool value)
    {
        foreach (var VARIABLE in m_FightUIs)
        {
            VARIABLE.SetActive(value);
        }
    }
}
