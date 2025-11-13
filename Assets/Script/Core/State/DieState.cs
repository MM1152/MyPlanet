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
        waveManager = GameObject.FindGameObjectWithTag(TagIds.WaveManager).GetComponent<WaveManager>();   
       #if DEBUG_MODE
        Debug.Log("Die State Enter");  
         #endif
    }

    public void Execute()
    {
        var exp = ObjectPoolManager.Instance.SpawnObject<Exp>(2, enemy.expPrefab);   
        exp.transform.position = enemy.transform.position;
        exp.exp = enemy.enemyData.EXP;
        ObjectPoolManager.Instance.Despawn(1, enemy.gameObject);  
        waveManager.currentWave.EnemyDefeated();                  
        enemy.IsDead = true;
    }

    public void Exit()
    {
       #if DEBUG_MODE
        Debug.Log("Die State Exit");  
         #endif
    }
}
