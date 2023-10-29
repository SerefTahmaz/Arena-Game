using DefaultNamespace;
using DemoBlast.Utils;
using UnityEngine;

public class cGameManager : cSingleton<cGameManager>
{
    [SerializeField] private cPlayerIconList m_PlayerIconList;

    public cPlayerIconList PlayerIconList => m_PlayerIconList;
}