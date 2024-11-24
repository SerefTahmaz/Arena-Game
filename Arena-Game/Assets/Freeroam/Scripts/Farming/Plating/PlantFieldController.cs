using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.UI.PopUps.InfoPopUp;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Gameplay;
using Gameplay.Farming;
using UnityEngine;

public class PlantFieldController : MonoBehaviour, IPlantHolderHandler
{
    [SerializeField] private PlantFieldItemSO m_PlantFieldItemSo;
    [SerializeField] private List<InteractablePlant> m_PlantHolders;

    public PlantFieldItemSO PlantFieldItemSo => m_PlantFieldItemSo;

    public List<InteractablePlant> PlantHolders => m_PlantHolders;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
       
        PlantFieldItemSo.Load();
        for (var index = 0; index < PlantHolders.Count; index++)
        {
            if (index >= PlantFieldItemSo.PlantList.Count)
            {
                PlantFieldItemSo.PlantList.Add(null);
                PlantFieldItemSo.Save();
            }
            
            var insPlaceHolder = PlantHolders[index];
            insPlaceHolder.Init( this);
            if (PlantFieldItemSo.PlantList[index] != null)
            {
                insPlaceHolder.SpawnPlant(PlantFieldItemSo.PlantList[index]);
            }
        }
    }

    public void HandleOnPlayerEnter(PlantHolderController plantHolderController)
    {
    }

    public void HandleOnPlayerExit(PlantHolderController plantHolderController)
    {
    }

    public void SaveSeed(InteractablePlant interactablePlant, PlantItemSO plantItemSo)
    {
        PlantFieldItemSo.Load();
        PlantFieldItemSo.PlantList[PlantHolders.IndexOf(interactablePlant)]  = plantItemSo;
        PlantFieldItemSo.Save();
    }
}

public interface IPlantHolderHandler
{
    public void HandleOnPlayerEnter(PlantHolderController plantHolderController);
    public void HandleOnPlayerExit(PlantHolderController plantHolderController);
}