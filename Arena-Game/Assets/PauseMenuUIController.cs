using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseMenuUIController : MonoBehaviour
{
    [SerializeField] private cButton m_MainMenuButton;
    [SerializeField] private UnityEvent m_OnMainMenuButtonClicked;
    
    // Start is called before the first frame update
    void Start()
    {
        m_MainMenuButton.OnClickEvent.AddListener(HandleMainMenuClicked);
    }

    private void HandleMainMenuClicked()
    {
        m_OnMainMenuButtonClicked.Invoke();
        cGameManager.Instance.LeaveGame();
    }
}
