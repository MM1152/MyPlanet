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
        ObjectPoolManager.Instance.Despawn(1 , enemy.gameObject);
        enemy.IsDead = true;
    }

    public void Exit()
    {
       #if DEBUG_MODE
        Debug.Log("Die State Exit");  
         #endif
    }
}
