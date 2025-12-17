using Cysharp.Threading.Tasks;
using UnityEngine;


public class OneTwoMoveAttack : IMove
{
    enum OneTwoMoveState
    {
        Start,
        ToAttackPoint,
        Attacking,
        WaitingAttack,
        WaitingMove,
    }

    private Enemy enemy;
    private Rect screenBounds;
    private Vector2 direction;
    public Vector2 Direction => direction;
    private Vector2 topPosition;
    private Vector2 middlePosition;
    private Vector2 bottomPosition;
    private OneTwoMoveState currentState;
    private bool _useVerticalPattern = false;
    public bool useVerticalPattern => _useVerticalPattern;
    private float initDelayTime = 2f;
    private float delayTime;
    private bool started = false;
    private float centerYAlpha = 1f;
    public void Init(Enemy enemy)
    {
        this.enemy = enemy;
        delayTime = initDelayTime;
        screenBounds = Utils.GetScreenBounds();
        var centerY = (enemy.target.transform.position.y + screenBounds.yMax) / 2;
        middlePosition = new Vector2(enemy.target.transform.position.x, centerY);
        topPosition = middlePosition + Vector2.up * centerYAlpha;
        bottomPosition = middlePosition + Vector2.down * centerYAlpha;
        currentState = OneTwoMoveState.Start;
        Debug.Log($"[OneTwoMoveAttack] 초기화 완료. 중간위치: {middlePosition}, 상단위치: {topPosition}, 하단위치: {bottomPosition}");
        Debug.Log("초기화 진행");
    }

    public void Move(Enemy enemy)
    {
        float step = enemy.speed * Time.deltaTime;

        switch (currentState)
        {
            case OneTwoMoveState.Start:
                MoveToCenter(enemy, step);
                break;
            case OneTwoMoveState.ToAttackPoint:
                MoveToAttackPoint(enemy, step);
                break;
            case OneTwoMoveState.Attacking:
                SwitchAttack(enemy);
                break;
            case OneTwoMoveState.WaitingAttack:
                WaitAttack();
                break;
            case OneTwoMoveState.WaitingMove:
                WaitAtPoint();
                break;
        }
    }

    private void MoveToCenter(Enemy enemy, float step)
    {
        Debug.Log("MoveToCenter");
        direction = (middlePosition - (Vector2)enemy.transform.position).normalized;
        enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, middlePosition, step);

        if (Vector2.Distance(enemy.transform.position, middlePosition) < 0.1f)
        {
            Debug.Log("Start -> Waiting");
            currentState = OneTwoMoveState.WaitingMove;
            return;
        }
    }

    private void MoveToAttackPoint(Enemy enemy, float step)
    {
        Debug.Log("MoveToAttackPoint");
        if (started == false) started = true;
        Vector2 targetPos = _useVerticalPattern ? topPosition : bottomPosition;
        direction = (targetPos - (Vector2)enemy.transform.position).normalized;
        enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, targetPos, step);
        if (Vector2.Distance(enemy.transform.position, targetPos) < 0.1f)
        {
            _useVerticalPattern = !_useVerticalPattern;
            currentState = OneTwoMoveState.WaitingAttack;
            Debug.Log("ToAttackPoint -> Waiting");
            return;
        }
    }

    private void SwitchAttack(Enemy enemy)
    {
        RotateTowardsTarget(enemy);
        enemy.stateMachine.ChangeState(enemy.stateMachine.attackState);
        currentState = OneTwoMoveState.WaitingMove;
    }

    private void RotateTowardsTarget(Enemy enemy)
    {
        if (enemy.target == null) return;

        direction = (enemy.target.transform.position - enemy.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        enemy.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void WaitAtPoint()
    {
        direction = Vector2.zero;
        delayTime -= Time.deltaTime * 2;
        if (delayTime <= 0f)
        {
            delayTime = initDelayTime;
            currentState = OneTwoMoveState.ToAttackPoint;
            Debug.Log("Waiting -> ToAttackPoint");
            return;
        }
    }



    private void WaitAttack()
    {
        direction = Vector2.zero;
        delayTime -= Time.deltaTime;
        if (delayTime <= 0f)
        {
            delayTime = initDelayTime;
            currentState = OneTwoMoveState.Attacking;
            Debug.Log("Waiting -> Attacking");
            return;
        }
    }

}
