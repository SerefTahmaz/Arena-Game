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
        m_PlayerCharacter = GameplayStatics.GetPlayerCharacter();
        m_PlantFieldItemSo.Load();
        for (var index = 0; index < m_PlantHolders.Count; index++)
        {
            var insPlaceHolder = m_PlantHolders[index];
            insPlaceHolder.Init(m_PlantFieldItemSo.PlantList[index]);
        }
    }

    public void HandleOnPlayerEnter(PlantHolderController plantHolderController)
    {
        RequestSeedSelection();
    }

    private async UniTask RequestSeedSelection()
    {
        var ins = GlobalFactory.SelectorPopUpFactory.Create();
        var selectedSeed = await ins.WaitForSelection();
        if (selectedSeed != null)
        {
            Debug.Log($"Selected seed name {selectedSeed.ItemName}");
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