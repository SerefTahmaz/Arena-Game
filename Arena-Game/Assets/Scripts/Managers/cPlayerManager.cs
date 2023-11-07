using System;
using System.Collections;
using System.Collections.Generic;
using DemoBlast.Utils;
using UnityEngine;

public class cPlayerManager : cSingleton<cPlayerManager>
{
    public Action<Transform> m_OwnerPlayerSpawn= delegate {  };

    public List<GameObject> m_Players = new List<GameObject>();

    public void DestroyPlayers()
    {
        
    }
}
