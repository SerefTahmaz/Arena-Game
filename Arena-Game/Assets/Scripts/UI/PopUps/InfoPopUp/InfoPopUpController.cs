using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace ArenaGame.UI.PopUps.InfoPopUp
{
    public class InfoPopUpController : PopUpController, IInfoPopUpController
    {
        [SerializeField] private cView m_View;
        [SerializeField] private cButton m_Button;
        [SerializeField] private TMP_Text m_Text;
        
        protected bool waitLock;

        private void Awake()
        {
            m_Button.OnClickEvent.AddListener(HandleOK);
        }

        public async UniTask Init(string value)
        {
            m_View.Deactivate(true);
            m_View.Activate();
            m_Text.text = value;
            
            waitLock = true;
            await UniTask.WaitWhile((() => waitLock));
        }
        
        private void HandleOK()
        {
            waitLock = false;
            gameObject.SetActive(false);
        }
    }

    public interface IInfoPopUpController : IPopUpController
    {
        UniTask Init(string value);
    }
}