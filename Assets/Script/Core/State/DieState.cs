using System.Runtime.Serialization;
using UnityEngine;

public class DieState : IState
{
    private Enemy enemy;
    private WaveManager waveManager;

    public DieState(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public void Enter()
    {
        enemy.IsDead = true;
        var exp = ObjectPoolManager.Instance.SpawnObject<Exp>(2, enemy.expPrefab);
        exp.transform.position = enemy.transform.position;
        exp.exp = enemy.enemyData.EXP;

        enemy.waveManager.currentWave.EnemyDefeated();
        ObjectPoolManager.Instance.Despawn(1, enemy.gameObject);
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {
#if DEBUG_MODE
        Debug.Log("Die State Exit");  
#endif
    }
}
