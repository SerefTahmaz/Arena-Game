using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level/Level List")]
public class cLevelListSO : ScriptableObject
{
    [SerializeField] private List<cLevelSO> m_LevelList;

    public List<cLevelSO> LevelList => m_LevelList;
}