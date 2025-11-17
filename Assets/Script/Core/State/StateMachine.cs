using UnityEngine;
using System;

[Serializable]
public class StateMachine
{
    public IState currentState { get; private set; }
    public IdleState idleState;
    public WalkState walkState;
    public AttackState attackState; 
    public DieState dieState;

    public StateMachine(Enemy enemy)
    {
        idleState = new IdleState(enemy);
        walkState = new WalkState(enemy);
        attackState = new AttackState(enemy);   
        dieState = new DieState(enemy);
    }

    public void Init(IState startingState)
    {
        ChangeState(startingState);
        startingState.Enter();
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;

        if (currentState != null)
        {
            currentState.Enter();
        }
    }

    private void ExecuteCurrentState()
    {
        if (currentState != null)
        {
            currentState.Execute();
        }
    }

}
