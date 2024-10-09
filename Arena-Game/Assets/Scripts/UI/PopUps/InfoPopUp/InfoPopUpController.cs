using System;
using TMPro;
using UnityEngine;

namespace ArenaGame.UI.PopUps.InfoPopUp
{
    public class InfoPopUpController : PopUpController, IInfoPopUpController
    {
        [SerializeField] private cView m_View;
        [SerializeField] private cButton m_Button;
        [SerializeField] private TMP_Text m_Text;

        private void Awake()
        {
            m_Button.OnClickEvent.AddListener(HandleOK);
        }

        public void Init(string value)
        {
            m_View.Deactivate(true);
            m_View.Activate();
            m_Text.text = value;
        }
        
        private void HandleOK()
        {
            gameObject.SetActive(false);
        }
    }

    public interface IInfoPopUpController : IPopUpController
    {
        void Init(string value);
    }
}