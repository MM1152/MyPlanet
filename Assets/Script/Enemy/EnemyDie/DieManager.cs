using System;
using System.Collections.Generic;
public static class DieManager
{
    public static Dictionary<int, Func<BaseDie>> dieTable = new Dictionary<int, Func<BaseDie>>()
    {
         { 0, () => new BaseDie() },
         { 3006, () => new ExplosionDie() },
         { 3007, () => new HealDie()},
         { 3010, () => new SpawnDie()},
         { 3015, () => new SplitbornDie()},
    };

    public static BaseDie GetDie(int key)
    {
        if (dieTable.ContainsKey(key))
        {
            return dieTable[key]();
        }
        return dieTable[0]();
    }
}
