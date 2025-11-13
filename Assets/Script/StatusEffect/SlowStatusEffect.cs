using UnityEngine;

public class SlowStatusEffect : IStatusEffect
{
    public StatusEffectType EffectType => effectType;
    private StatusEffectType effectType = StatusEffectType.Slow;

    private IDamageAble target;
    private IMoveAble moveAble;
    private float duration;
    private float timer;
    private float slowPercent;

    public SlowStatusEffect(float duration, float slowPercent)
    {
        this.duration = duration;
        this.slowPercent = slowPercent;
    }

    public void Apply(IDamageAble target)
    {
        if(target is IMoveAble moveAble)
        {
            this.target = target;
            this.moveAble = moveAble;
            this.moveAble.CurrentSpeed = moveAble.BaseSpeed * (1 - slowPercent);
            Debug.Log("Slow");
        }
    }

    public IStatusEffect DeepCopy()
    {
        return new SlowStatusEffect(duration, slowPercent);
    }

    public void Remove()
    {
        this.moveAble.CurrentSpeed = moveAble.BaseSpeed;
        Debug.Log("EndSlow");
        target.StatusEffect.Remove(this);
    }

    public void ResetStatusEffect()
    {
        timer = 0;
    }

    public void Update(float deltaTime)
    {
        timer += deltaTime;
        if(timer >= duration)
        {
            Remove();
        }
    }
}