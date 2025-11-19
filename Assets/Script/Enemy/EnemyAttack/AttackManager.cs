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
        Debug.LogError("Enemy에 해당하는 공격방식이 없음 ");
#endif
        return null;
    }
}
