using UnityEngine;

public enum BossMoveState
{
    MoveToCenter,
    Attack,
}

public class BossSimpleMove : IMove
{
    private GameObject target;
    private Rect screenBounds;
    private BossMoveState currentPattern;
    private Vector2 centerPoint;
    private Vector2 direction;

    public Vector2 Direction => direction;

    public void Init(Enemy enemy)
    {
        target = enemy.GetTarget();
        if(enemy.WaveManager != null)
        {
            screenBounds = enemy.WaveManager.ScreenBounds;
        }
        else
        {
            var camera = Camera.main;

            if (camera == null) return;

            var zDistance = Mathf.Abs(camera.transform.position.z);

            var bottomLeft = camera.ScreenToWorldPoint(new Vector3(0, 0, zDistance));
            var topRight = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, zDistance));

            screenBounds = new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
        }
        var centerY = (target.transform.position.y + screenBounds.yMax) / 2;
        centerPoint = new Vector2(target.transform.position.x, centerY);
        currentPattern = BossMoveState.MoveToCenter;
    }

    public void Move(Enemy enemy)
    {
        if (target == null)
        {
            enemy.stateMachine.ChangeState(enemy.stateMachine.idleState);
            direction = Vector2.zero;
            return;
        }
  
        float step = enemy.CurrentSpeed * Time.deltaTime;

        switch (currentPattern)
        {
            case BossMoveState.MoveToCenter:
                MoveToCenter(enemy, step);
                break;

            case BossMoveState.Attack:
                Attack(enemy);
                break;
        }
    }
    
    private void RotateTowardsTarget(Enemy enemy)
    {
        if (target == null) return;

        direction = (target.transform.position - enemy.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        enemy.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void MoveToCenter(Enemy enemy, float step)
    {
        direction = (centerPoint - (Vector2)enemy.transform.position).normalized;
        enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, centerPoint, step);

        if (Vector2.Distance(enemy.transform.position, centerPoint) < 0.1f)
        {
            currentPattern = BossMoveState.Attack;
            return;
        }
    }

    private void Attack(Enemy enemy)
    {
        RotateTowardsTarget(enemy);
        enemy.stateMachine.ChangeState(enemy.stateMachine.attackState);
    }
}