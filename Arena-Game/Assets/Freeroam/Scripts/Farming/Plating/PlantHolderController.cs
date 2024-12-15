using System;
using System.Collections;
using System.Collections.Generic;
using ArenaGame.Utils;
using AudioSystem;
using DefaultNamespace;
using Gameplay.Farming;
using UnityEngine;
using UnityEngine.Serialization;

public class PlantHolderController : MonoBehaviour
{
    [SerializeField] private Transform m_PlantHolderPivot;
    [SerializeField] private GameObject m_AvailableSpotVfx;
    [SerializeField] private GameObject m_CollectSpotVfx;
    [SerializeField] private AudioClip m_PlantSoundClip;
     
    private PlantController m_InsPlantController;
    
    public PlantItemSO SeedItemSo { get; set; }
    public PlantController InsPlantController => m_InsPlantController;

    public void PlantWithSeed(SeedItemSO seedItemSo)
    {
        var insPlant = seedItemSo.GivePlantItemInsSO();
        insPlant.CreationDate = DateTime.Now;
        insPlant.Save();
        SpawnPlant(insPlant);
        SoundManager.PlayOneShot2D(m_PlantSoundClip);
    }

    public void Init()
    {
        m_AvailableSpotVfx.SetActive(true);
    }
    
    public void SpawnPlant(PlantItemSO plantSO)
    {
        SeedItemSo = plantSO;
        m_InsPlantController = Instantiate(plantSO.PlantItemTemplate.PlantPrefab);
        InsPlantController.transform.SetParentResetTransform(m_PlantHolderPivot);
        InsPlantController.Init(plantSO);
        
        m_AvailableSpotVfx.SetActive(false);
        plantSO.Load();
        switch (plantSO.PlantState)
        {
            case PlantState.NewBorn:
                break;
            case PlantState.FullyGrown:
                m_CollectSpotVfx.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void DestroyPlant()
    {
        Destroy(InsPlantController.gameObject);
        m_InsPlantController = null;
        SeedItemSo = null;
        m_AvailableSpotVfx.SetActive(true);
        m_CollectSpotVfx.SetActive(false);
    }
}