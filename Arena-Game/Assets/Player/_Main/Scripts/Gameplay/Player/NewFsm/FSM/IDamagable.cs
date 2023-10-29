using UnityEngine;

public interface IDamagable
{
    public void Damage(int amount, Vector3 pos,bool isHeavyDamage);
    public int TeamID { get; }
}