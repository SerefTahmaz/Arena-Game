using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Gameplay;
using UnityEngine;

public class PlantFieldController : MonoBehaviour, IPlantHolderHandler
{
    [SerializeField] private PlantFieldItemSO m_PlantFieldItemSo;
    [SerializeField] private List<PlantHolderController> m_PlantHolders;
    [SerializeField] private PlantItemSO m_EmptyItem;

    private CharacterSO m_PlayerCharacter;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        m_PlayerCharacter = GameplayStatics.GetPlayerCharacterSO();
        m_PlantFieldItemSo.Load();
        for (var index = 0; index < m_PlantHolders.Count; index++)
        {
            if (index >= m_PlantFieldItemSo.PlantList.Count)
            {
                m_PlantFieldItemSo.PlantList.Add(null);
                m_PlantFieldItemSo.Save();
            }
            
            var insPlaceHolder = m_PlantHolders[index];
            insPlaceHolder.Init( this);
            if (m_PlantFieldItemSo.PlantList[index] != null)
            {
                insPlaceHolder.SpawnPlant(m_PlantFieldItemSo.PlantList[index]);
            }
        }
    }

    public void HandleOnPlayerEnter(PlantHolderController plantHolderController)
    {
        HandleOnPlayerEnterAsync(plantHolderController);
    }

    private async UniTask HandleOnPlayerEnterAsync(PlantHolderController plantHolderController)
    {
        if (plantHolderController.SeedItemSo)
        {
            switch (plantHolderController.SeedItemSo.PlantState)
            {
                case PlantState.NewBorn:
                    var infoPopUp = GlobalFactory.InfoPopUpFactory.Create();
                    infoPopUp.Init("Plant is growing. Check back later");
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
                        m_PlantFieldItemSo.PlantList[m_PlantHolders.IndexOf(plantHolderController)] = null;
                        m_PlantFieldItemSo.Save();
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
            RequestSeedSelection(plantHolderController);
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
            // await UniTask.DelayFrame(1);
            m_PlantFieldItemSo.Load();
            m_PlantFieldItemSo.PlantList[m_PlantHolders.IndexOf(plantHolderController)] = plantHolderController.InsPlantController.PlantItemSo;
            m_PlantFieldItemSo.Save();
            
            // m_PlayerCharacter.Load();
            // m_PlayerCharacter.RemoveInventory(selectedSeed);
        }
    }

    public void HandleOnPlayerExit(PlantHolderController plantHolderController)
    {
        throw new NotImplementedException();
    }
}

public interface IPlantHolderHandler
{
    public void HandleOnPlayerEnter(PlantHolderController plantHolderController);
    public void HandleOnPlayerExit(PlantHolderController plantHolderController);
}