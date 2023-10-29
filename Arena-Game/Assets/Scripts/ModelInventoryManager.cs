using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelInventoryManager : MonoBehaviour
{
    [SerializeField] private Transform _model;
    [SerializeField] private GameObject _sword;

    public void OnItemWear(List<int> indexs)
    {
        foreach (var VARIABLE in indexs)
        {
            if (VARIABLE == 100)
            {
                _sword.SetActive(true);
            }
            else
            {
                _model.GetChild(VARIABLE).gameObject.SetActive(true);
            }
        }
    }

    public void OnItemRemove(List<int> indexs)
    {
        foreach (var VARIABLE in indexs)
        {
            if (VARIABLE == 100)
            {
                _sword.SetActive(false);
            }
            else
            {
                _model.GetChild(VARIABLE).gameObject.SetActive(false);
            }
        }
    }
}
