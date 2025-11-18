using System.Runtime.Serialization;
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
    }

    public void Execute()
    {
        enemy.WaveManager.totalEnemyCount--;
        var exp = Managers.ObjectPoolManager.SpawnObject<Exp>(PoolsId.Exp);   
        exp.transform.position = enemy.transform.position;
        exp.exp = enemy.enemyData.EXP;

        Managers.ObjectPoolManager.Despawn(PoolsId.Enemy, enemy.gameObject);            
        enemy.IsDead = true;
    }

    public void Exit()
    {
    }
}
