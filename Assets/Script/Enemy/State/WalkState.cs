using UnityEngine;

public class WalkState : IState
{
    private Enemy enemy;
    private GameObject target;
    public WalkState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        target = enemy.GetTarget();
        enemy.speed = enemy.CurrentSpeed;
    }

    public void Execute()
    {
        if(target == null)
        {           
            enemy.stateMachine.ChangeState(enemy.stateMachine.idleState);
            return; 
        }

        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, target.transform.position, enemy.CurrentSpeed * Time.deltaTime);
       var distance = Vector3.Distance(enemy.transform.position, target.transform.position);
        if(distance <= enemy.attackrange&&enemy.enemyType!=EnemyType.Melee)
        {
            enemy.stateMachine.ChangeState(enemy.stateMachine.attackState);
        }   
    }

    public void Exit()
    {   
    }
}
