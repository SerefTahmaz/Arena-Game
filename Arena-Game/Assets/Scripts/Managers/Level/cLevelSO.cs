using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level/Level")]
public class cLevelSO : ScriptableObject
{
    [SerializeField] private GameObject m_PrefabToSpawn;


    public GameObject PrefabToSpawn => m_PrefabToSpawn;
}
