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
        if (target == null)
        {
            enemy.stateMachine.ChangeState(enemy.stateMachine.idleState);
            return;
        }

        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, target.transform.position, enemy.CurrentSpeed * Time.deltaTime);
        var distance = Vector3.Distance(enemy.transform.position, target.transform.position);

        Vector2 dir = target.transform.position - enemy.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        enemy.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        if (distance <= enemy.attackrange && enemy.enemyType != EnemyType.Melee)
        {
            enemy.stateMachine.ChangeState(enemy.stateMachine.attackState);
        }
    }

    public void Exit()
    {
    }
}
