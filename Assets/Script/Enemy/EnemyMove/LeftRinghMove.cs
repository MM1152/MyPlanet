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

    private Vector2 direction;

    public Vector2 Direction => direction;

    public void Init(Enemy enemy)
    {

        screenBounds = Utils.GetScreenBounds();
        enemyCollider = enemy.GetComponent<Collider2D>();
        enemyBounds = enemyCollider.bounds;
        target = enemy.GetTarget();
        var y = enemy.transform.position.y > 0f ? screenBounds.yMax : screenBounds.yMin;
        var centerY = (target.transform.position.y + y) / 2;
        startPos = new Vector2(enemy.transform.position.x, centerY);
        leftPoint = new Vector2(screenBounds.xMin + enemyBounds.extents.x + Vector2.right.x, centerY);
        rightPoint = new Vector2(screenBounds.xMax - enemyBounds.extents.x - Vector2.right.x, centerY);
    }

    public void Move(Enemy enemy)
    {
        float step = enemy.speed * Time.deltaTime;
        Debug.Log($"LeftRinghMove Move Pattern: {currentPattern}");
        Debug.Log($"{enemy.ElementType}");
        switch (currentPattern)
        {
            case LeftRightMovePattern.ToCenter:
                MoveToCenter(enemy, step);
                break;
            case LeftRightMovePattern.Moving:
                EnemyMoving(enemy, step);
                break;
        }
        RotateTowardsTarget(enemy);
    }
    private void MoveToCenter(Enemy enemy, float step)
    {
        enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, startPos, step);
        Debug.Log($"시작지점 이동중");
        if (Vector2.Distance(enemy.transform.position, startPos) < 0.1f)
        {
            Debug.Log("사이드 이동 시작");
            currentPattern = LeftRightMovePattern.Moving;
            return;
        }
    }
    private void EnemyMoving(Enemy enemy, float step)
    {
        var targetPos = isMovingRight ? rightPoint : leftPoint;
        direction = (targetPos - (Vector2)enemy.transform.position).normalized;
        enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, targetPos, step);
        Debug.Log("사이드 이동중");
        if (Vector2.Distance(enemy.transform.position, targetPos) < 0.1f)
        {
            Debug.Log("방향꺽기");
            isMovingRight = !isMovingRight;
            return;
        }
    }
    private void RotateTowardsTarget(Enemy enemy)
    {
        if (target == null) return;

        direction = target.transform.position - enemy.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        enemy.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}