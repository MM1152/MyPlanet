using System;
using System.Collections.Generic;
using UnityEngine;

public static class AttackManager
{
    public static Dictionary<EnemyType, Func<IAttack>> attackTable = new Dictionary<EnemyType, Func<IAttack>>()
    {
        { EnemyType.Melee, () => new OneTimeMeleeAttacker() },
        { EnemyType.Ranged  , () => new ShotAttack() },
        {EnemyType.EliteMonster, () => new EliteMonsterAttack() },
        {EnemyType.Boss, () => new BossAttack() },
    };
    public static  IAttack GetAttack(EnemyType key = EnemyType.None)
    {
        if (attackTable.ContainsKey(key))
        {
            return attackTable[key]();
        }
#if DEBUG_MODE
        Debug.LogError("Enemy 타입에 해당하는 공격방식이 없지만 기본 근접공격 넣어드려요");
#endif
        return attackTable[EnemyType.Melee]();
    }  
}
