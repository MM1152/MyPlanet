using System.Collections.Generic;
using UnityEngine;

public class AbilityManager
{
    public Dictionary<int, BaseAbility> abilityTable = new Dictionary<int, BaseAbility>()
    {
         { 3007, new BarrierAbility() },
         { 3021, new HealZoneAbility() },
         { 3022, new  PelletBoosterAbility() },
         { 3023, new BarrierRefillAbility() },
         { 3024, new  RangeBoostAbility() },
         { 3025, new  PlayerKillSplitAbility() },
    };

    public BaseAbility GetAbility(int key)
    {
        if (abilityTable.ContainsKey(key))
        {
            return abilityTable[key];
        }
        return null;
    }
}
