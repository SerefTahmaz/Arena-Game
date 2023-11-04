using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cDummyDamageable : MonoBehaviour, IDamagable
{
    [SerializeField] private Transform m_FocusPoint;

    public void Damage(DamageWrapper damageWrapper)
    {
    }

    public int TeamID { get; }
    public Transform FocusPoint => m_FocusPoint;
}
