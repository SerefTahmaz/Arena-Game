using UnityEngine;

public interface IDamagable
{
    public void Damage(DamageWrapper damageWrapper);
    public int TeamID { get; }
    public Transform FocusPoint { get; }
}

public class DamageWrapper
{
    public int amount;
    public Vector3 pos;
    public bool isHeavyDamage;
    public Transform damager;
    public cCharacter Instigator;
}