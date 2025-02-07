using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PVE Level List", menuName = "Game/PVELevel/Level List")]
public class PVELevelListSO : ScriptableObject
{
    [SerializeField] private List<PVELevelSO> m_LevelList;

    public List<PVELevelSO> LevelList => m_LevelList;
}