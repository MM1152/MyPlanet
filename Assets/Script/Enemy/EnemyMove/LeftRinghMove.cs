using UnityEngine;

public class LeftRinghMove : IMove
{
    private Rect screenBounds;

    private Vector2 startPos;

    private Vector2 leftPoint;

    private Vector2 rightPoint;

    private bool startcenter = false;

    private float speed;

    private Collider2D enemyCollider;

    private Bounds enemyBounds;

    private bool isMovingRight = false;

    public void Init(Enemy enemy)
    {
        screenBounds = enemy.WaveManager.ScreenBounds;
        speed = enemy.speed;
        enemyCollider = enemy.GetComponent<Collider2D>();
        enemyBounds = enemyCollider.bounds;
        var y = enemy.transform.position.y > 0f ? screenBounds.yMax : screenBounds.yMin;
        var centerY = (enemy.GetTarget().transform.position.y + y) / 2;
        startPos = new Vector2(enemy.transform.position.x, centerY);
        leftPoint = new Vector2(screenBounds.xMin + enemyBounds.extents.x, centerY);
        rightPoint = new Vector2(screenBounds.xMax - enemyBounds.extents.x, centerY);
    }

    public void Move(Enemy enemy)
    {
        float step = speed * Time.deltaTime;

        if (!startcenter)
        {
            enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, startPos, step);
            if (Vector2.Distance(enemy.transform.position, startPos) < 0.1f)
            {
                startcenter = true;
                enemy.stateMachine.ChangeState(enemy.stateMachine.attackState);
            }
        }
        else
        {
            if (Mathf.Abs(enemy.transform.position.x - leftPoint.x) < 0.1f)
            {
                isMovingRight = true;
                enemy.stateMachine.ChangeState(enemy.stateMachine.attackState);
            }
            else if (Mathf.Abs(enemy.transform.position.x - rightPoint.x) < 0.1f)
            {
                isMovingRight = false;
                enemy.stateMachine.ChangeState(enemy.stateMachine.attackState);
            }
            else
            {
                if (enemy.transform.position.x < screenBounds.xMin)
                {
                    isMovingRight = true;
                }
                else if (enemy.transform.position.x > screenBounds.xMax)
                {
                    isMovingRight = false;
                }
            }

            var targetPos = isMovingRight ? rightPoint : leftPoint;
            enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, targetPos, step);
        }
    }
}