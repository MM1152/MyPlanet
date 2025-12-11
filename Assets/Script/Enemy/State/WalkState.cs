using UnityEngine;

public class WalkState : IState
{
    private Enemy enemy;
    // private GameObject target;
    public WalkState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {        
    }

    public void Execute()
    {       
        if(enemy.GetTarget() != null && Vector3.Distance(enemy.GetTarget().transform.position , enemy.transform.position) <= enemy.attackRange)
        {
            enemy.stateMachine.ChangeState(enemy.stateMachine.attackState);
            return;
        }
        enemy.move.Move(enemy);
    }

    public void Exit()
    {
    }
}
