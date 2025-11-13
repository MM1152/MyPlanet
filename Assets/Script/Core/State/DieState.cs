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
       #if DEBUG_MODE
        Debug.Log("Die State Enter");  
         #endif
    }

    public void Execute()
    {
        var exp = Managers.ObjectPoolManager.SpawnObject<Exp>(2, enemy.expPrefab);   
        exp.transform.position = enemy.transform.position;
        exp.exp = enemy.enemyData.EXP;
        Managers.ObjectPoolManager.Despawn(1, enemy.gameObject);            
        enemy.IsDead = true;
    }

    public void Exit()
    {
       #if DEBUG_MODE
        Debug.Log("Die State Exit");  
         #endif
    }
}
