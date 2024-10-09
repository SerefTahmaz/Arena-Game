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
            var insPlaceHolder = m_PlantHolders[index];
            insPlaceHolder.Init( this);
            if (index < m_PlantFieldItemSo.PlantList.Count  && m_PlantFieldItemSo.PlantList[index] != null)
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
            GameplayStatics.GetPlayerCharacterSO().Load();
            GameplayStatics.GetPlayerCharacterSO().RemoveInventory(selectedSeed);
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