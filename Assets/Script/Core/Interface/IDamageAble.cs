using UnityEngine;

public interface IDamageAble
{
    public bool IsDead { get; }

    public void OnDamage(int damage);
    public void OnDead();
}
