using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PVELevel", menuName = "Game/PVELevel/Level")]
public class PVELevelSO : ScriptableObject
{
    [SerializeField] private Sprite m_Icon;
    [SerializeField] private string m_NameText;
    [SerializeField] private string m_SceneName;

    public Sprite Icon => m_Icon;

    public string NameText => m_NameText;

    public string SceneName => m_SceneName;
}
