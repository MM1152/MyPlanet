using UnityEngine;

public class SimpleMove : IMove
{
    private GameObject target;
    private Vector2 direction;

    public Vector2 Direction => direction;

    public void Init(Enemy enemy)
    {
        target = enemy.GetTarget();
    }

    public void Move(Enemy enemy)
    {
        if (target == null)
        {
            enemy.stateMachine.ChangeState(enemy.stateMachine.idleState);
            direction = Vector2.zero;
            return;
        }
        var distance = Vector3.Distance(enemy.transform.position, target.transform.position);

        if (distance <= enemy.attackRange && enemy.enemyType != EnemyType.Melee)
        {
            direction = Vector2.zero;
            enemy.stateMachine.ChangeState(enemy.stateMachine.attackState);
            return;
        }

        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, target.transform.position, enemy.CurrentSpeed * Time.deltaTime);
        RotateTowardsTarget(enemy);
    }

    private void RotateTowardsTarget(Enemy enemy)
    {
        if (target == null) return;

        direction = (target.transform.position - enemy.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        enemy.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
