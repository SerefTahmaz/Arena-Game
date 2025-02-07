using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Gameplay.Farming;
using UnityEngine;

public class PlantController : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_FullyGrownLayer;
    
    public PlantItemSO PlantItemSo { get; set; }
    
    public void Init(PlantItemSO plantSo)
    {
        PlantItemSo = plantSo;
        PlantItemSo.Load();
        var timeDifference = DateTime.Now - PlantItemSo.CreationDate;
        if (timeDifference.Days >= 1)
        {
            PlantItemSo.PlantState = PlantState.FullyGrown;
            PlantItemSo.Save();
        }
        
        switch (PlantItemSo.PlantState)
        {
            case PlantState.NewBorn:
                foreach (var VARIABLE in m_FullyGrownLayer)
                {
                    VARIABLE.SetActive(false);
                }
                break;
            case PlantState.FullyGrown:
                foreach (var VARIABLE in m_FullyGrownLayer)
                {
                    VARIABLE.SetActive(true);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public FoodItemSO GiveProducedFoodItemInsSO()
    {
        return PlantItemSo.GiveProducedFoodItemInsSO();
    }
}
