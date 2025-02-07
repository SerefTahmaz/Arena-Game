using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.UI;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class InteractionButtonController : MonoBehaviour
{
    [SerializeField] private GameObject m_Button;
    [SerializeField] private Image m_Image;
    [SerializeField] private Sprite m_PlantIcon;
    [SerializeField] private Sprite m_NpcIcon;

    private void Awake()
    {
        SetButton(false);
        PlayerInteractionHelper.Instance.Interactables.Updated += InteractablesOnUpdated;
        PlayerInteractionHelper.Instance.OnCurrentInteractableChanged += CurrentInteractableChanged;
    }

    private void CurrentInteractableChanged()
    {
        if (PlayerInteractionHelper.Instance.CurrentInteractable is InteractableNPC)
        {
            m_Image.sprite = m_NpcIcon;
        }
        if (PlayerInteractionHelper.Instance.CurrentInteractable is InteractablePlant)
        {
            m_Image.sprite = m_PlantIcon;
        }
    }

    private void InteractablesOnUpdated()
    {
        SetButton(PlayerInteractionHelper.Instance.Interactables.Count > 0);
    }

    public void SetButton(bool value)
    {
        m_Button.SetActive(value);
    }
}
