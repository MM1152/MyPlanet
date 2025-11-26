using UnityEngine;

enum LeftRightMovePattern
{
    ToCenter,
    Moving,
}

public class LeftRinghMove : IMove
{
    private Rect screenBounds;

    private Vector2 startPos;

    private Vector2 leftPoint;

    private Vector2 rightPoint;    

    private Collider2D enemyCollider;

    private Bounds enemyBounds;

    private LeftRightMovePattern currentPattern;

    private bool isMovingRight = false;

    private GameObject target;

    public void Init(Enemy enemy)
    {
        screenBounds = enemy.WaveManager.ScreenBounds;    
        enemyCollider = enemy.GetComponent<Collider2D>();
        enemyBounds = enemyCollider.bounds;
        target = enemy.GetTarget();
        var y = enemy.transform.position.y > 0f ? screenBounds.yMax : screenBounds.yMin;
        var centerY = (target.transform.position.y + y) / 2;
        startPos = new Vector2(enemy.transform.position.x, centerY);
        leftPoint = new Vector2(screenBounds.xMin + enemyBounds.extents.x+Vector2.right.x, centerY);
        rightPoint = new Vector2(screenBounds.xMax - enemyBounds.extents.x-Vector2.right.x, centerY);
    }

    public void Move(Enemy enemy)
    {
        float step = enemy.speed * Time.deltaTime;

        switch (currentPattern)
        {
            case LeftRightMovePattern.ToCenter:
                MoveToCenter(enemy, step);
                break;
            case LeftRightMovePattern.Moving:
                EnemyMoving(enemy, step);
                break;           
        }       
    }
    private void MoveToCenter(Enemy enemy, float step)
    {
        enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, startPos, step);
        if (Vector2.Distance(enemy.transform.position, startPos) < 0.1f)
        {
            currentPattern = LeftRightMovePattern.Moving;
            return;
        }
    }
    private void EnemyMoving(Enemy enemy, float step)
    {
        var targetPos = isMovingRight ? rightPoint : leftPoint;
        enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, targetPos, step);
        if(Vector2.Distance(enemy.transform.position, targetPos) < 0.1f)
        {
            RotateTowardsTarget(enemy);             
            isMovingRight = !isMovingRight;
            return;
        }
    }
     private void RotateTowardsTarget(Enemy enemy)
    {
        if (target == null) return;

        Vector2 dir = target.transform.position - enemy.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        enemy.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}