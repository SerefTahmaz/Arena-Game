using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class PlantController : MonoBehaviour
{
    [SerializeField] private GameObject m_FullyGrownLayer;
    
    public PlantItemSO PlantItemSo { get; set; }
    
    public void Init(PlantItemSO plantSo)
    {
        plantSo.Load();
        var timeDifference = DateTime.Now - plantSo.CreationDate;
        if (timeDifference.Days >= 1)
        {
            plantSo.PlantState = PlantState.FullyGrown;
            plantSo.Save();
        }
        
        switch (plantSo.PlantState)
        {
            case PlantState.NewBorn:
                m_FullyGrownLayer.SetActive(false);
                break;
            case PlantState.FullyGrown:
                m_FullyGrownLayer.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnDestroy()
    {
        if (PlantItemSo)
        {
            PlantItemSo.Save();
        }
    }
}
