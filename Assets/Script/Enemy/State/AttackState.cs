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
        enemy.attack.Attack(enemy); 
    }

    public void Exit()
    {
    }
}
