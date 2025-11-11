using UnityEngine;

public class WalkState : IState
{
    private Enemy enemy;
    public WalkState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
       #if DEBUG_MODE
        Debug.Log("Walk State Enter");  
         #endif
        
    }

    public void Execute()
    {
        enemy.speed = enemy.enemyData.Speed;
    }

    public void Exit()
    {
       #if DEBUG_MODE
        Debug.Log("Walk State Exit");  
         #endif     
    }
}
