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
        
        GameObject.Destroy(enemy.gameObject);
        isDead = true;
    }

    public void Exit()
    {
       #if DEBUG_MODE
        Debug.Log("Die State Exit");  
         #endif
    }
}
