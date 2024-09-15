using System.Collections;
using System.Collections.Generic;
using ArenaGame.Utils;
using UnityEngine;

public class cKillClosestDebug : MonoBehaviour
{
    public void OnClick()
    {
        var selected = FindObjectsOfType<cCharacterStateMachine>().RandomItem();
        selected.OnDamage(new DamageWrapper()
        {
            amount = 10000,
            Character = null,
            damager = transform,
            isHeavyDamage = false,
            pos = transform.position
        });
    }
}
