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
    private PlantController m_InsPlantController;
    
    public PlantItemSO SeedItemSo { get; set; }
    public PlantController InsPlantController => m_InsPlantController;

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
        m_InsPlantController = Instantiate(plantSO.PlantItemTemplate.PlantPrefab);
        InsPlantController.transform.SetParentResetTransform(m_PlantHolderPivot);
        InsPlantController.Init(plantSO);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out IPlayerMarker _))
        {
            m_PlantHolderHandler.HandleOnPlayerEnter(this);
        }
    }

    public void DestroyPlant()
    {
        Destroy(InsPlantController.gameObject);
        m_InsPlantController = null;
        SeedItemSo = null;
    }
}