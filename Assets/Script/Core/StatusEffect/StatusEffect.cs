using System;
using System.Collections.Generic;

[Flags] 
public enum StatusEffectType
{
    None = 0,
    Burn = 1 << 0,
    Stun = 1 << 1,
    Slow = 1 << 2,
    AttackSpeedUp = 1 << 3,
}

public class StatusEffect
{
    private List<IStatusEffect> status = new List<IStatusEffect>();
    private List<IStatusEffect> removeStatus = new List<IStatusEffect>();

    private StatusEffectType effectTypes = StatusEffectType.None;
    public StatusEffectType EffectTypes => effectTypes;

    private bool clearAllStatus = false;
    
    public void Init()
    {
        status.Clear();
        removeStatus.Clear();
        effectTypes = StatusEffectType.None;
        clearAllStatus = false;
    }

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

    // LateUpdate에서 실행해야됨
    public void Update(float deltaTime)
    {
        foreach(var statu in status)
        {
            if(!removeStatus.Contains(statu))
            {
                statu.Update(deltaTime);
            }
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

        if(clearAllStatus)
        {
            effectTypes = StatusEffectType.None;
            status.Clear();
        }
    }

    public void Remove(IStatusEffect status)
    {
        removeStatus.Add(status);
    }

    public void Clear()
    {
        clearAllStatus = true;
    }
}