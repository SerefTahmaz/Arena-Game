using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cDummyDamageable : MonoBehaviour, IDamagable
{
    [SerializeField] private Transform m_FocusPoint;

    public void Damage(int amount, Vector3 pos, bool isHeavyDamage)
    {
        throw new System.NotImplementedException();
    }

    public int TeamID { get; }
    public Transform FocusPoint => m_FocusPoint;
}
