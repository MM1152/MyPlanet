using UnityEngine;

public class AttackState : IState
{
    private Enemy enemy;

    public AttackState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    { 
    }

    public void Execute()
    {
       if(Vector2.Distance(enemy.transform.position, enemy.GetTarget().transform.position) > enemy.attackRange)
        {
            enemy.stateMachine.ChangeState(enemy.stateMachine.walkState);
            return;
        }

        enemy.attack.Attack(enemy); 
    }

    public void Exit()
    {
    }
}
