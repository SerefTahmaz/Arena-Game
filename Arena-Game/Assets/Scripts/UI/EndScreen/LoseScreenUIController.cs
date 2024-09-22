using ArenaGame.Currency;
using ArenaGame.Experience;
using ArenaGame.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace UI.EndScreen
{
    public class LoseScreenUIController : MonoBehaviour
    { 
        [SerializeField] private cButton m_ContinueButton;
        [SerializeField] private cView m_View;

        private void Awake()
        {
            m_ContinueButton.OnClickEvent.AddListener(HandleContinueClicked);
            m_View.OnActivateEvent.AddListener(OnViewActivated);
        }

        private void OnViewActivated()
        {
            OnViewActivatedAsync();
        }

        private async UniTask OnViewActivatedAsync()
        {
            m_ContinueButton.DOComplete();
            m_ContinueButton.DeActivate();
            m_ContinueButton.transform.localScale = Vector3.zero;
        
            ExperienceManager.LoseExperience(35);
            CurrencyManager.SpendCurrency(50);
            await UniTask.WaitForSeconds(0.5f);
        
            m_ContinueButton.Activate();
            m_ContinueButton.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        }

        private void HandleContinueClicked()
        {
            cGameManager.Instance.HandleLoseContinueButton();
        }
    }
}
