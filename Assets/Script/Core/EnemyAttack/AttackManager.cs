using System.Collections.Generic;
using UnityEngine;

public class AttackManager
{   
    public Dictionary<EnemyAttackKey, IAttack> attackTable = new Dictionary<EnemyAttackKey, IAttack>()
    {        
        { new EnemyAttackKey(EnemyType.Melee,ElementType.Fire,EnemyTier.Tier3), new OneTimeMeleeAttacker() },
    };

    public AttackManager(EnemyAttackKey key, out IAttack attack)
    {
       attack = GetAttack(key);
    }

    public IAttack GetAttack(EnemyAttackKey key)
    {
        if (attackTable.ContainsKey(key))
        {
            return attackTable[key];
        }
#if DEBUG_MODE
        Debug.LogError("Enemy에 해당하는 공격방식이 없음 ");
#endif
        return null;
    }

}
