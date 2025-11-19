using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class DieState : IState
{
    private Enemy enemy;

    public DieState(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public void Enter()
    {
       enemy.die.Init(enemy);   
    }

    public void Execute()
    {
        if (enemy.isKilledByPlayer)
        {
            var exp = Managers.ObjectPoolManager.SpawnObject<Exp>(PoolsId.Exp);
            exp.transform.position = enemy.transform.position;
            exp.exp = enemy.enemyData.EXP;
        }        
        enemy.die.Die(enemy);
    }

    public void Exit()
    {
    }
}
