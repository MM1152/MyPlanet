using UnityEngine;

public enum AbilityType
{
    None = -1,
    OnDamage = 0,
    OnUpdate = 1,
}

public abstract class BaseAbility
{
    protected Enemy enemy;
    public virtual AbilityType abilityType { get; }
    public virtual bool isActive { get; set; }

    public void SetEnemy(Enemy enemy)
    {
        this.enemy = enemy;
    }   

    public virtual int OnDamage(int damage) { return damage; }
    public virtual void OnUpdate(float deltaTime) { }
}
