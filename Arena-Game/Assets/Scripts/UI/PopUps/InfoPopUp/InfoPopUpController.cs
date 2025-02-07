using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace ArenaGame.UI.PopUps.InfoPopUp
{
    public class InfoPopUpController : PopUpController, IInfoPopUpController
    {
        private cView View => m_PopUpReferenceHelper.View;
        private cButton Button => m_PopUpReferenceHelper.YesButton;
        private TMP_Text Text => m_PopUpReferenceHelper.Text;
        
        protected bool waitLock;

        private void Awake()
        {
            Button.OnClickEvent.AddListener(HandleOK);
        }

        public async UniTask Init(string value)
        {
            View.Deactivate(true);
            View.Activate();
            Text.text = value;
            
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