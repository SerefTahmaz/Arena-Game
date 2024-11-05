using System.Collections;
using System.Collections.Generic;
using ArenaGame.UI;
using UnityEngine;

public class InteractionButtonController : MonoBehaviour
{
    [SerializeField] private GameObject m_Button;

    private bool IsEnabled;

    // Update is called once per frame
    void Update()
    {
        if (InputManager.Instance._onInteractionEvent?.GetInvocationList().Length > 0)
        {
            if (!IsEnabled)
            {
                IsEnabled = true;
                m_Button.SetActive(true);
            }
        }
        else
        {
            if (IsEnabled)
            {
                IsEnabled = false;
                m_Button.SetActive(false);
            }
        }
    }
}
