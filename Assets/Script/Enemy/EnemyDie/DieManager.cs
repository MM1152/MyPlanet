using System.Collections.Generic;
using UnityEngine;

public class DieManager
{
    public Dictionary<int, BaseDie> dieTable = new Dictionary<int, BaseDie>()
    {
         { 0, new BaseDie() },
         { 3006, new ExplosionDie() },
         { 3007, new HealDie()},
         { 3010, new SpawnDie()},
         { 3015, new SplitbornDie()},
    };

    public BaseDie GetDie(int key)
    {
        if (dieTable.ContainsKey(key))
        {
            return dieTable[key];
        }
        return dieTable[0];
    }
}
