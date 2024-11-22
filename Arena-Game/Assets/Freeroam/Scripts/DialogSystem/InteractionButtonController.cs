using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.UI;
using DefaultNamespace;
using UnityEngine;

public class InteractionButtonController : MonoBehaviour
{
    [SerializeField] private GameObject m_Button;

    private void Awake()
    {
        SetButton(false);
        PlayerInteractionHelper.Instance.InteractableNpcs.Updated += InteractableNpcsOnUpdated;
    }

    private void InteractableNpcsOnUpdated()
    {
        SetButton(PlayerInteractionHelper.Instance.InteractableNpcs.Count > 0);
    }

    public void SetButton(bool value)
    {
        m_Button.SetActive(value);
    }
}
