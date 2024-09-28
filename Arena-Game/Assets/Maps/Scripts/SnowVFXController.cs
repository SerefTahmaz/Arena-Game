using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowVFXController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (cGameManager.Instance.m_OwnerPlayer != null)
        {
            transform.position = cGameManager.Instance.m_OwnerPlayer.position;
        }
    }
}
