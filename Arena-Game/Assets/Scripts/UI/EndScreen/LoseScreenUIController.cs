using ArenaGame.Currency;
using ArenaGame.Experience;
using ArenaGame.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI.EndScreen
{
    public class LoseScreenUIController : MonoBehaviour
    { 
        [SerializeField] private cButton m_ContinueButton;
        [SerializeField] private cView m_View;
        [SerializeField] private TMP_Text m_LoseExpText;
        [SerializeField] private TMP_Text m_LoseCurrencyText;
        

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
           
            SetLoseCurrencies();

            await UniTask.WaitForSeconds(0.5f);
        
            m_ContinueButton.Activate();
            m_ContinueButton.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        }

        private void SetLoseCurrencies()
        {
            var currentMap = MapManager.Instance.GetCurrentMap();
            var loseExp = currentMap.LoseExp;
            var loseCurrency = currentMap.LoseCurrency;
            m_LoseExpText.text = "-"+loseExp.ToString();
            m_LoseCurrencyText.text = "-"+loseCurrency.ToString();
            ExperienceManager.LoseExperience(loseExp);
            CurrencyManager.SpendCurrency(loseCurrency);
        }

        private void HandleContinueClicked()
        {
            cGameManager.Instance.HandleLoseContinueButton();
        }
    }
}
