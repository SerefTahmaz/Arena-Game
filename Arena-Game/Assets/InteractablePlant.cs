using System;
using ArenaGame.UI;
using ArenaGame.UI.PopUps.InfoPopUp;
using Cysharp.Threading.Tasks;
using Gameplay;
using UnityEngine;

namespace DefaultNamespace
{
    public class InteractablePlant : BaseInteractable
    {
        [SerializeField] private PlantHolderController m_PlantHolderController;
        
        private PlantFieldController m_PlantFieldController;
        private CharacterSO m_PlayerCharacter;

        public override InteractionHelper InteractionHelper => PlayerInteractionHelper.Instance;
        
        public void Init(PlantFieldController plantFieldController)
        {
            m_PlayerCharacter = GameplayStatics.GetPlayerCharacterSO();
            m_PlantFieldController = plantFieldController;
            m_PlantHolderController.Init();
        }

        protected override void HandleInteractionEvent()
        {
            if(FindObjectOfType<PlantFieldCollectPopUpController>() || FindObjectOfType<InfoPopUpController>()) return;
            base.HandleInteractionEvent();
        }

        protected override async UniTask  HandleInteraction()
        {
            GameplayStatics.SetPlayerInput(false);
            await ShowPopUp(m_PlantHolderController);
            GameplayStatics.SetPlayerInput(true);
        }
        
        private async UniTask ShowPopUp(PlantHolderController plantHolderController)
        {
            if (plantHolderController.SeedItemSo)
            {
                switch (plantHolderController.SeedItemSo.PlantState)
                {
                    case PlantState.NewBorn:
                        var infoPopUp = GlobalFactory.InfoPopUpFactory.Create();
                        await infoPopUp.Init("Plant is growing. Check back later");
                        break;
                    case PlantState.FullyGrown:
                        var collectPopUp = GlobalFactory.PlantFieldCollectPopUpFactory.Create();
                        var isSuccessfull = await collectPopUp.Init();
                        if (isSuccessfull)
                        {
                            m_PlayerCharacter.Load();
                            var producedFoodItemSOIns = plantHolderController.InsPlantController.GiveProducedFoodItemInsSO();
                            m_PlayerCharacter.AddInventory(producedFoodItemSOIns);
                            plantHolderController.DestroyPlant();
                            m_PlantFieldController.SaveSeed(this, null);
                        }
                        else
                        {
                        
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                await RequestSeedSelection(plantHolderController);
            }
        }
        
        private async UniTask RequestSeedSelection(PlantHolderController plantHolderController)
        {
            var ins = GlobalFactory.SelectorPopUpFactory.Create();
            var selectedSeed = await ins.WaitForSelection();
            if (selectedSeed != null)
            {
                Debug.Log($"Selected seed name {selectedSeed.ItemName}");
                plantHolderController.PlantWithSeed(selectedSeed);
                m_PlantFieldController.SaveSeed(this, plantHolderController.InsPlantController.PlantItemSo);
                m_PlayerCharacter.Load();
                m_PlayerCharacter.RemoveInventory(selectedSeed);
            }
        }

        public void SpawnPlant(PlantItemSO plant)
        {
            m_PlantHolderController.SpawnPlant(plant);
        }
    }
}