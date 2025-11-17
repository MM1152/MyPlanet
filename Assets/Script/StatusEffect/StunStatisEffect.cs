using UnityEngine;

public class StunStatusEffect : IStatusEffect
{
    public StatusEffectType EffectType => effectType;
    private StatusEffectType effectType = StatusEffectType.Stun;

    private IDamageAble target;
    private IMoveAble moveAble;

    private float duration;
    private float curDuration;
    public StunStatusEffect(float duration)
    {
        this.duration = duration;
    }

    public void Apply(IDamageAble target)
    {
        if(target is IMoveAble moveAble)
        {
            this.target = target;
            this.moveAble = moveAble;
            this.moveAble.CurrentSpeed = 0;
            this.moveAble.IsStun = true;
            curDuration = duration;
        }
    }

    public IStatusEffect DeepCopy()
    {
        return new StunStatusEffect(duration);
    }

    public void Remove()
    {
        this.moveAble.CurrentSpeed = moveAble.BaseSpeed;
        Debug.Log("Stun 해제");
        this.moveAble.IsStun = false;
        target.StatusEffect.Remove(this);
    }

    public void ResetStatusEffect()
    {
        curDuration = duration;
    }

    public void Update(float deltaTime)
    {
        curDuration -= deltaTime;

        if(curDuration <= 0)
        {
            Remove();
        }
    }
}