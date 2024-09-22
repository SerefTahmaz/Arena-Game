using System;
using UnityEngine;

namespace ArenaGame.UI.PopUps.InfoPopUp
{
    public class InfoPopUpController : MonoBehaviour, IInfoPopUpController
    {
        [SerializeField] private cView m_View;
        [SerializeField] private cButton m_Button;

        private void Awake()
        {
            m_Button.OnClickEvent.AddListener(HandleOK);
        }

        public void Init(string value)
        {
            m_View.Deactivate(true);
            m_View.Activate();
        }
        
        private void HandleOK()
        {
            gameObject.SetActive(false);
        }
    }

    public interface IInfoPopUpController
    {
        void Init(string value);
    }
}