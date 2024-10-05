using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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
            m_Button.gameObject.SetActive(false);
        }

        public async UniTask ActivateButton()
        {
            m_Button.gameObject.SetActive(true);
            m_Button.DeActivate();
            m_Button.transform.localScale = Vector3.zero;
            await m_Button.transform.DOScale(1, 0.5f);
            m_Button.Activate();
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