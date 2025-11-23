using System.Collections.Generic;
using UnityEngine;

public class DieManager
{
    public Dictionary<int, BaseDie> dieTable = new Dictionary<int, BaseDie>()
    {
         { 3, new ExplosionDie() },
         {4, new HealDie()},
         {5, new SpawnDie()},
    };

    public BaseDie GetDie(int key)
    {
        if (dieTable.ContainsKey(key))
        {
            return dieTable[key];
        }
#if DEBUG_MODE
        Debug.LogError("Enemy에 해당하는 죽음방식이 없음 ");
#endif
        return null;
    }


    public DieManager(int key, out BaseDie die)
    {
        if( dieTable.ContainsKey(key))
        {
            die = dieTable[key];
            return;
        }
        die = new BaseDie();
    }
}
