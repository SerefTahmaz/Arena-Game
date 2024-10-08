using System.Linq;
using Cysharp.Threading.Tasks;
using Gameplay;
using Gameplay.Farming;
using UnityEngine;

namespace ArenaGame.UI.MenuInventory
{
    public class SeedSelectorPopUpController : ItemViewer,ISeedSelectorPopUpController
    {
        [SerializeField] private CharacterSO m_CharacterSo;
        [SerializeField] private cButton m_DismissButton;

        private SeedItemSO m_SelectedSeedItemSo;
        private bool m_HasPlayerSelected;
        
        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            base.Init();
            m_DismissButton.OnClickEvent.AddListener((() =>
            {
                m_SelectedSeedItemSo = null;
                m_HasPlayerSelected = true;
            }));
        }

        public async UniTask<SeedItemSO> WaitForSelection()
        {
            m_CharacterSo.Load();
            Refresh(m_CharacterSo.InventoryList.Where((so => so as SeedItemSO)).ToList());
            m_HasPlayerSelected = false;
            m_SelectedSeedItemSo = null;
            await UniTask.WaitUntil((() => m_HasPlayerSelected));
            gameObject.SetActive(false);
            return m_SelectedSeedItemSo;
        }

        public override void HandleClick(MenuInventoryItemController menuInventoryItemController)
        {
            base.HandleClick(menuInventoryItemController);
            m_SelectedSeedItemSo = (menuInventoryItemController as ConsumableInventoryItemController).itemSO as SeedItemSO;
            m_HasPlayerSelected = true;
        }
        
    }
    
    public interface ISeedSelectorPopUpController : IPopUpController
    {
        UniTask<SeedItemSO> WaitForSelection();
    }
}