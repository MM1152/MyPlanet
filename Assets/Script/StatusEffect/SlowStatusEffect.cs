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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="duration">지속 시간 파라미터</param>
    /// <param name="slowPercent"> 0 ~ 1 사이로 정규화된 값 넣어줘야함</param>
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
        }
    }

    public IStatusEffect DeepCopy()
    {
        return new SlowStatusEffect(duration, slowPercent);
    }

    public void Remove()
    {
        this.moveAble.CurrentSpeed = moveAble.BaseSpeed;
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