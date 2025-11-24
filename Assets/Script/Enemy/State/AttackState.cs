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
        if (enemy.attackRange <= 0 || Vector2.Distance(enemy.transform.position, enemy.GetTarget().transform.position) <= enemy.attackRange)
        {
            enemy.attack.Attack(enemy);
            return;
        }

        enemy.stateMachine.ChangeState(enemy.stateMachine.walkState);
    }

    public void Exit()
    {
    }
}
