using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cKickPlayerButton : MonoBehaviour
{
    [SerializeField] private cPlayerUnit m_PlayerUnit;

    public void KickPlayer()
    {
        m_PlayerUnit.KickPlayer();
    }
}
