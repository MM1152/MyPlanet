using System.Collections.Generic;
using UnityEngine;

public class AbilityManager
{
    public Dictionary<int, BaseAbility> abilityTable = new Dictionary<int, BaseAbility>()
    {
         { 3, new BarrierAbility() },
         { 12, new HealZoneAbility() },
         {13,new BarrierRefillAbility() },
         {14,new  PelletBoosterAbility() },
         {15,new  RangeBoostAbility() },

    };

    public BaseAbility GetAbility(int key)
    {
        if (abilityTable.ContainsKey(key))
        {
            return abilityTable[key];
        }
        return null;
    }
    public AbilityManager(int key, out BaseAbility ability)
    {
        if (abilityTable.ContainsKey(key))
        {
            ability = abilityTable[key];
            return;
        }
        ability = null;
    }
}
