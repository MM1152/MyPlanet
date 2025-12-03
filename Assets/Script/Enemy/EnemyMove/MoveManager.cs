using System;
using System.Collections.Generic;
using UnityEngine;

public static class MoveManager 
{
  private static Dictionary<EnemyType, Func<IMove>> moveTable = new Dictionary<EnemyType, Func<IMove>>()
    {        
        { EnemyType.Melee, () => new SimpleMove() },                
        { EnemyType.Ranged, () => new SimpleMove() },
        { EnemyType.EliteMonster, () => new BaseElementalMove() },
        { EnemyType.Boss, () => new BossSimpleMove() },   
    };

    public static IMove GetMove(EnemyType Id)
    {
        if (moveTable.ContainsKey(Id))
        {
            return moveTable[Id]();
        }
        return moveTable[0]();    
    }
}
