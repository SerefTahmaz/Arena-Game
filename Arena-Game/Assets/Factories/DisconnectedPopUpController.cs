using System;
using TMPro;
using UnityEngine;

namespace ArenaGame.UI.PopUps.DisconnectedPopUp
{
    public class DisconnectedPopUpController : MonoBehaviour, IDisconnectedPopUpController
    {
        [SerializeField] private cView m_View;
        [SerializeField] private cButton m_Button;
        [SerializeField] private TMP_Text m_ReasonText;

        private void Awake()
        {
            m_Button.OnClickEvent.AddListener(HandleOK);
        }

        public void Init(string value)
        {
            m_View.Deactivate(true);
            m_View.Activate();
            m_ReasonText.text = value;
        }
        
        private void HandleOK()
        {
            gameObject.SetActive(false);
            cGameManager.Instance.OnMainMenuButtonClick();
        }
    }

    public interface IDisconnectedPopUpController
    {
        void Init(string value);
    }
}