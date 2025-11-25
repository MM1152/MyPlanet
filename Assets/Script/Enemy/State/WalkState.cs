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
        enemy.move.Init(enemy);
    }

    public void Execute()
    {       
        enemy.move.Move(enemy);
    }

    public void Exit()
    {
    }
}
