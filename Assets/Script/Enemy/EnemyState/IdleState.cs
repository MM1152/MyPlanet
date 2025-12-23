using UnityEngine;

public class IdleState : IState
{
    private Enemy enemy;
   
    public IdleState(Enemy enemy)
    {
        this.enemy = enemy;
    }

     public void Enter()
    {
    }

    public void Execute()
    {
        if(enemy.GetTarget() != null)
        {
            enemy.stateMachine.ChangeState(enemy.stateMachine.walkState);
        }   
    }

    public void Exit()
    {           
    }
}
