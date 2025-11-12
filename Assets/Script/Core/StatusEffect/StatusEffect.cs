using System;
using System.Collections.Generic;

[Flags] 
public enum StatusEffectType
{
    None = 0,
    Burn = 1 << 0,
    Stun = 1 << 1,
    Slow = 1 << 2,
}

public class StatusEffect
{
    private List<IStatusEffect> status = new List<IStatusEffect>();
    private List<IStatusEffect> removeStatus = new List<IStatusEffect>();
    private StatusEffectType effectTypes = StatusEffectType.None;

    public void Apply(IStatusEffect status, IDamageAble target)
    {
        if (status == null) return;

        // 이미 걸린 상태이상인지 검사
        if ((effectTypes & status.EffectType) != 0)
        {
            for(int i = 0; i < this.status.Count; i++)
            {
                if (this.status[i].EffectType == status.EffectType)
                {
                    this.status[i].ResetStatusEffect();
                    break;
                }
            }
        }
        else
        {
            effectTypes |= status.EffectType;
            status.Apply(target);
            this.status.Add(status);
        }

    }

    public void Update(float deltaTime)
    {
        foreach(var statu in status)
        {
            statu.Update(deltaTime);
        }
        
        if(removeStatus.Count > 0)
        {
            foreach (var removeStatus in removeStatus)
            {
                effectTypes ^= removeStatus.EffectType;
                status.Remove(removeStatus);
            }
            removeStatus.Clear();
        }
    }

    public void Remove(IStatusEffect status)
    {
        removeStatus.Add(status);
    }

    public void Clear()
    {
        status.Clear();
    }
}