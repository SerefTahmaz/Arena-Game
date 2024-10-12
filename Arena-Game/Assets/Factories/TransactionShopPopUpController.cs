using System;
using Gameplay;
using UI.Shop;
using UnityEngine;
 
namespace Factories
{
    public class TransactionShopPopUpController : PopUpController, ITransactionShopPopUpController
    {
        [SerializeField] private TransactionShopController m_PlayerShop;
        [SerializeField] private TransactionShopController m_NpcShop;
        [SerializeField] private cButton m_DismissButton;
        
        public Action OnDismissed { get; set; }

        private void Awake()
        {
            m_DismissButton.OnClickEvent.AddListener(HandleDismissButtonClicked);
        }

        private void HandleDismissButtonClicked()
        {
            gameObject.SetActive(false);
            OnDismissed?.Invoke();
            m_PlayerShop.CleanUp();
            m_NpcShop.CleanUp();
        }

        public void Init(CharacterSO playerSO, CharacterSO npcSO)
        {
            m_PlayerShop.Init(playerSO, npcSO,true);
            m_NpcShop.Init(npcSO,playerSO,false);
        }
    }

    public interface ITransactionShopPopUpController : IPopUpController
    {
        public Action OnDismissed { get; set; }
        public void Init(CharacterSO playerSO, CharacterSO npcSO);
    }
}