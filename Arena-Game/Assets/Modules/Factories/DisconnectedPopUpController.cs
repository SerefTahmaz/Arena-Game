using System;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace ArenaGame.UI.PopUps.DisconnectedPopUp
{
    public class DisconnectedPopUpController : BasePopUp, IDisconnectedPopUpController
    {
        private cView View => m_PopUpReferenceHelper.View;
        private cButton Button => m_PopUpReferenceHelper.YesButton; 
        private TMP_Text ReasonText => m_PopUpReferenceHelper.Text;

        private void Awake()
        {
            Button.OnClickEvent.AddListener(HandleOK);
        }

        public void Init(string value)
        {
            View.Deactivate(true);
            View.Activate();
            ReasonText.text = value;
            Button.gameObject.SetActive(false);
        }

        public async UniTask ActivateButton()
        {
            Button.gameObject.SetActive(true);
            Button.DeActivate();
            Button.transform.localScale = Vector3.zero;
            await Button.transform.DOScale(1, 0.5f);
            Button.Activate();
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
        UniTask ActivateButton();
    }
}