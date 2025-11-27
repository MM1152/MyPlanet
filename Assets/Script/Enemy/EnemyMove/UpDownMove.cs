using System.Security;
using Cysharp.Threading.Tasks.Triggers;
using Unity.VisualScripting;
using UnityEngine;

public enum UpDownPattern
{
    ToAttackPoint,
    Attacking,
    ReturnPoint,
    Waiting,
}

public class UpDownMove : IMove
{
    private Rect screenBounds;

    private GameObject target;

    private Vector2 upAttackPos;
    private Vector2 downAttackPos;

    private Vector2 upPoint;

    private Vector2 downPoint;

    private float delayTime;

    private Collider2D enemyCollider;

    private Bounds enemyBounds;

    private bool isMovingUp = false;

    private UpDownPattern currentPattern;
    public void Init(Enemy enemy)
    {
        screenBounds = enemy.WaveManager.ScreenBounds;
        enemyCollider = enemy.GetComponent<Collider2D>();
        enemyBounds = enemyCollider.bounds;
        currentPattern = UpDownPattern.ToAttackPoint;
        target = enemy.GetTarget();

        var upCenterY = (target.transform.position.y + screenBounds.yMax) / 2;
        var downCenterY = (target.transform.position.y + screenBounds.yMin) / 2;
        upAttackPos = new Vector2(enemy.transform.position.x, upCenterY);
        downAttackPos = new Vector2(enemy.transform.position.x, downCenterY);
        upPoint = new Vector2(enemy.transform.position.x, screenBounds.yMax + enemyBounds.extents.y+Vector2.up.y);
        downPoint = new Vector2(enemy.transform.position.x, screenBounds.yMin - enemyBounds.extents.y-Vector2.up.y);
        isMovingUp = enemy.transform.position.y > upAttackPos.y ? true : false;
    }

    public void PositionSet(Enemy enemy)
    {
        enemy.transform.position = isMovingUp ? upPoint : downPoint;
        currentPattern = UpDownPattern.ToAttackPoint;
    }

    public void Move(Enemy enemy)
    {
        float step = enemy.speed * Time.deltaTime;
     
        switch (currentPattern)
        {
            case UpDownPattern.ToAttackPoint:
                MoveToAttackPoint(enemy, step);
                break;
            case UpDownPattern.Attacking:
                SwitchAttack(enemy);
                break;
            case UpDownPattern.ReturnPoint:
                MoveReturnPoint(enemy, step);
                break;
            case UpDownPattern.Waiting:
                WaitAtPoint();
                break;
        }
    }

    private void RotateTowardsTarget(Enemy enemy)
    {
        if (target == null) return;

        Vector2 dir = target.transform.position - enemy.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        enemy.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void SwitchAttack(Enemy enemy)
    {
        RotateTowardsTarget(enemy);
        enemy.stateMachine.ChangeState(enemy.stateMachine.attackState);
        currentPattern = UpDownPattern.ReturnPoint;
    }

    private void MoveToAttackPoint(Enemy enemy, float step)
    {
        Vector2 targetPos = isMovingUp ? upAttackPos : downAttackPos;
        enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, targetPos, step);
        if (Vector2.Distance(enemy.transform.position, targetPos) < 0.1f)
        {
            currentPattern = UpDownPattern.Attacking;
            return;
        }
    }

    private void MoveReturnPoint(Enemy enemy, float step)
    {
        var targetPos = isMovingUp ? upPoint : downPoint;
        enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, targetPos, step);
        if (Vector2.Distance(enemy.transform.position, targetPos) < 0.1f)
        {
            delayTime = 3f;
            isMovingUp = !isMovingUp;
            enemy.transform.position = isMovingUp ? upPoint : downPoint;
            currentPattern = UpDownPattern.Waiting;
            return;
        }
    }

    private void WaitAtPoint()
    {
        delayTime -= Time.deltaTime;
        if (delayTime <= 0f)
        {
            currentPattern = UpDownPattern.ToAttackPoint;
            return;
        }
    }
}
