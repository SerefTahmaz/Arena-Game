using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Gameplay.Farming;
using UnityEngine;

public class PlantHolderController : MonoBehaviour
{
    public void PlantWithSeed(SeedItemSO seedItemSo)
    {
        var insPlant = seedItemSo.GivePlantItemInsSO();
        insPlant.CreationDate = DateTime.Now;
        SpawnPlant(insPlant);
    }

    public void Init(PlantItemSO plantSO)
    {
        SpawnPlant(plantSO);
    }
    
    private void SpawnPlant(PlantItemSO plantSO)
    {
        var insPlant = plantSO.PlantItemTemplate.PlantPrefab;
        insPlant.Init(plantSO);
    }
}