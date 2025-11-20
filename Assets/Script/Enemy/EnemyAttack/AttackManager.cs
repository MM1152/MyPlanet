using System.Collections.Generic;
using UnityEngine;

public class AttackManager
{
    public Dictionary<EnemyType, IAttack> attackTable = new Dictionary<EnemyType, IAttack>()
    {
        { EnemyType.Melee, new OneTimeMeleeAttacker() },
        { EnemyType.Ranged  , new ShotAttack() },
    };

    public AttackManager(EnemyType key, out IAttack attack)
    {
        attack = GetAttack(key);
    }

    public IAttack GetAttack(EnemyType key)
    {
        if (attackTable.ContainsKey(key))
        {
            return attackTable[key];
        }
#if DEBUG_MODE
        Debug.LogError("Enemy 타입에 해당하는 공격방식이 없지만 기본 근접공격 넣어드려요");
#endif
        return attackTable[EnemyType.Melee];
    }
}
