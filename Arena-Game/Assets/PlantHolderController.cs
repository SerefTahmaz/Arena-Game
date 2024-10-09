using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Utils;
using DefaultNamespace;
using Gameplay.Farming;
using UnityEngine;
using UnityEngine.Serialization;

public class PlantHolderController : MonoBehaviour
{
    [SerializeField] private Transform m_PlantHolderPivot;
    [SerializeField] private GameObject m_AvailableSpotVfx;
    [SerializeField] private GameObject m_CollectSpotVfx;
     
    private IPlantHolderHandler m_PlantHolderHandler;
    
    public PlantItemSO SeedItemSo { get; set; }
    
    public void PlantWithSeed(SeedItemSO seedItemSo)
    {
        var insPlant = seedItemSo.GivePlantItemInsSO();
        insPlant.CreationDate = DateTime.Now;
        insPlant.Save();
        SpawnPlant(insPlant);
    }

    public void Init(IPlantHolderHandler plantHolderHandler)
    {
        m_PlantHolderHandler = plantHolderHandler;
    }
    
    public void SpawnPlant(PlantItemSO plantSO)
    {
        SeedItemSo = plantSO;
        var insPlant = Instantiate(plantSO.PlantItemTemplate.PlantPrefab);
        insPlant.transform.SetParentResetTransform(m_PlantHolderPivot);
        insPlant.Init(plantSO);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out IPlayerMarker _))
        {
            m_PlantHolderHandler.HandleOnPlayerEnter(this);
        }
    }
}