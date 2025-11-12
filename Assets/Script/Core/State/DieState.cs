using UnityEngine;

public class DieState : IState
{
    private Enemy enemy;    
    public bool isDead { get; private set; } 
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
        ObjectPoolManager.SpawnObject(2,enemy.expPrefab, enemy.transform.position, Quaternion.identity);   
        ObjectPoolManager.Despawn(1, enemy.gameObject);            
        isDead = true;
    }

    public void Exit()
    {
       #if DEBUG_MODE
        Debug.Log("Die State Exit");  
         #endif
    }
}
