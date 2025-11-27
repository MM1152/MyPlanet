using System.Collections.Generic;
using UnityEngine;

public enum CornerMovePattern
{
    ToAttackPoint,
    Attacking,
    MoveingNextCorner,
    Waiting
}

public class CornerWrapMove : IMove
{
    private Rect screenBounds;

    private GameObject target;

    private float delayTime = 3f;

    private Collider2D enemyCollider;

    private Bounds enemyBounds;

    private float centerY;

    private float upY;
    private float downY;

    Vector2 leftUpPoint;
    Vector2 leftDownPoint;
    Vector2 rightUpPoint;
    Vector2 rightDownPoint;

    Vector2 leftUpExitPoint;
    Vector2 leftDownExitPoint;
    Vector2 rightUpExitPoint;
    Vector2 rightDownExitPoint;

    private bool isLeftMoving = false;
    private bool isMovingUp = false;

    private CornerMovePattern currentPattern;    

    private Vector2 attackTargetPos;

    private Dictionary<Vector2, Vector2> cornerExitPoints;


    public void Init(Enemy enemy)
    {
        screenBounds = enemy.WaveManager.ScreenBounds;
        enemyCollider = enemy.GetComponent<Collider2D>();
        enemyBounds = enemyCollider.bounds;
        currentPattern = CornerMovePattern.ToAttackPoint;
        target = enemy.GetTarget();

        upY = (screenBounds.yMax + target.transform.position.y) / 2;
        downY = (screenBounds.yMin + target.transform.position.y) / 2;

        leftUpPoint = new Vector2(screenBounds.xMin + enemyBounds.extents.x + Vector2.right.x, upY);
        leftDownPoint = new Vector2(screenBounds.xMin + enemyBounds.extents.x + Vector2.right.x, downY);
        rightUpPoint = new Vector2(screenBounds.xMax - enemyBounds.extents.x + Vector2.left.x, upY);
        rightDownPoint = new Vector2(screenBounds.xMax - enemyBounds.extents.x + Vector2.left.x, downY);

        leftUpExitPoint = new Vector2(screenBounds.xMin - enemyBounds.extents.x - Vector2.right.x, upY);
        leftDownExitPoint = new Vector2(screenBounds.xMin - enemyBounds.extents.x - Vector2.right.x, downY);
        rightUpExitPoint = new Vector2(screenBounds.xMax + enemyBounds.extents.x + Vector2.right.x, upY);
        rightDownExitPoint = new Vector2(screenBounds.xMax + enemyBounds.extents.x + Vector2.right.x, downY);
       
        isLeftMoving = enemy.transform.position.x > screenBounds.center.x ? true : false;
        isMovingUp = enemy.transform.position.y > screenBounds.center.y ? true : false;

        attackTargetPos = (isLeftMoving, isMovingUp) switch
        {
            (true, true) => rightUpPoint,
            (true, false) => rightDownPoint,
            (false, true) => leftUpPoint,
            (false, false) => leftDownPoint,
        };

        cornerExitPoints = new Dictionary<Vector2, Vector2>()
        {
            { leftUpPoint, leftDownExitPoint },
            { leftDownPoint, leftUpExitPoint },
            { rightUpPoint, rightDownExitPoint },
            { rightDownPoint, rightUpExitPoint },
        };
    }

    public void Move(Enemy enemy)
    {
        float step = enemy.speed * Time.deltaTime;

        switch (currentPattern)
        {
            case CornerMovePattern.ToAttackPoint:
                MoveToAttackPoint(enemy, step);
                break;
            case CornerMovePattern.Attacking:
                SwitchAttack(enemy);
                break;
            case CornerMovePattern.MoveingNextCorner:
                MoveNextPoint(enemy, step);
                break;
            case CornerMovePattern.Waiting:
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
        currentPattern = CornerMovePattern.MoveingNextCorner;
    }

    private void MoveToAttackPoint(Enemy enemy, float step)
    {
        enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, attackTargetPos, step);
        if (Vector2.Distance(enemy.transform.position, attackTargetPos) < 0.1f)
        {
            currentPattern = CornerMovePattern.Attacking;
            return;
        }
    }

    private void MoveNextPoint(Enemy enemy, float step)
    {
        Vector2 moveToPos = enemy.transform.position.x > leftUpExitPoint.x ? Vector2.left : Vector2.right;
        enemy.transform.position += (Vector3)(moveToPos * step);
        if (enemy.transform.position.x < leftUpExitPoint.x ||
         enemy.transform.position.x > rightUpExitPoint.x)
        {            
            currentPattern = CornerMovePattern.Waiting;
            enemy.transform.position = GetExitPoint(attackTargetPos);
            delayTime = 3f;
            return;
        }
    }

    private void WaitAtPoint()
    {
        delayTime -= Time.deltaTime;
        if (delayTime <= 0f)
        {
            isMovingUp = !isMovingUp; 
            attackTargetPos = (isLeftMoving,isMovingUp) switch
            {
                (true, true) => rightUpPoint,
                (true, false) => rightDownPoint,
                (false, true) => leftUpPoint,
                (false, false) => leftDownPoint,
            };
            currentPattern = CornerMovePattern.ToAttackPoint;
            return;
        }
    }

    private Vector2 GetExitPoint(Vector2 currentPoint)
    {
        if (cornerExitPoints.TryGetValue(currentPoint, out Vector2 exitPoint))
        {
            return exitPoint;
        }
        return currentPoint;
    }
}
