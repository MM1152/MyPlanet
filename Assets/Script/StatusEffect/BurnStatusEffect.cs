using UnityEngine;

public class BurnStatusEffect : IStatusEffect
{
    private float duration;
    private float timer;
    private float tickInterval;
    private float currentTick;
    private int damage;
    private IDamageAble target;

    public StatusEffectType EffectType => effectType;
    private StatusEffectType effectType = StatusEffectType.Burn;

    public BurnStatusEffect(float duration, float tickInterval, int damage)
    {
        this.duration = duration;
        this.tickInterval = tickInterval;
        this.damage = damage;
    }

    public IStatusEffect DeepCopy()
    {
        return new BurnStatusEffect(duration, tickInterval, damage);
    }

    public void Apply(IDamageAble target)
    {
        this.target = target;
    }

    public void Remove()
    {
        target.StatusEffect.Remove(this);
    }

    public void Update(float deltaTime)
    {
        currentTick += deltaTime;   
        timer += deltaTime;
        if (tickInterval <= currentTick)
        {
            currentTick = 0;
            target.OnDamage(damage);
            Debug.Log($"화상 데미지 {damage}");
        }

        if(timer >= duration)
        {
            Remove();
            Debug.Log($"화상 끝 {damage}");
        }
    }

    public void ResetStatusEffect()
    {
        timer = 0;
    }
}