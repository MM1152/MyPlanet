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
        if (enemy.isKilledByPlayer)
        {
            var exp = Managers.ObjectPoolManager.SpawnObject<Exp>(PoolsId.Exp);
            exp.transform.position = enemy.transform.position;
            exp.exp = enemy.enemyData.EXP;
        }

        enemy.WaveManager.totalEnemyCount--;
        enemy.IsDead = true;
        Managers.ObjectPoolManager.Despawn(PoolsId.Enemy, enemy.gameObject);
    }

    public void Exit()
    {
    }
}
