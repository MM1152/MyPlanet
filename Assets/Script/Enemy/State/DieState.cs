using System.Runtime.Serialization;
using Unity.VisualScripting;
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
       enemy.die.Init(enemy);   
    }

    public void Execute()
    {    
        enemy.die.Die(enemy);
    }

    public void Exit()
    {
    }
}
