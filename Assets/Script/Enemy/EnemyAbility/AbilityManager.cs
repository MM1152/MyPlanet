using System;
using System.Collections.Generic;
using UnityEngine;

public static class AbilityManager
{
    public static Dictionary<int, Func<BaseAbility>> abilityTable = new Dictionary<int, Func<BaseAbility>>()
    {
         { 3008, () => new BarrierAbility() },
         { 3021, () => new HealZoneAbility() },
         { 3022, () => new  PelletBoosterAbility() },
         { 3023, () => new BarrierRefillAbility() },
         { 3024, () => new  RangeBoostAbility() },
         { 3025, () => new  PlayerKillSplitAbility() },
         {3042, () => new FortifiedBarrierAbility() },
    };

    public static BaseAbility GetAbility(int key)
    {
        if (abilityTable.ContainsKey(key))
        {
            return abilityTable[key]();
        }
        return null;
    }
}
