using UnityEngine;

public interface IDamageAble
{
    public StatusEffect StatusEffect { get; }
    public bool IsDead { get; }
    public ElementType ElementType { get; }

    public void OnDamage(int damage);
    public void OnDead();
}
