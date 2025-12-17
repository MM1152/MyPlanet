using UnityEngine;
using Cysharp.Threading.Tasks;
public class CounterLaserAbility : BaseAbility
{
    public override AbilityType abilityType => AbilityType.OnDamage;
    public override bool isActive { get; set; }
    private bool initialized = false;
    private LayerMask targetLayer = LayerMask.GetMask("Player"); // 타겟 레
    private bool inLaserAttack = false;
    private LineRenderer lineRenderer;
    private float laserDuration = 0f;
    private float laserMaxDuration = 2f;
    private ElementType targetElementType => ElementType.Dark; // 타겟 속성 타입 어둠으로 고정 테이블 

    public override void SetEnemy(Enemy enemy)
    {
        base.SetEnemy(enemy);
        lineRenderer = enemy.enemyLineRenderer;
        lineRenderer.enabled = false;
        isActive = true;
    }

    public override int OnDamage(int damage)
    {
        if (inLaserAttack) return damage;

        if (enemy.LastAttackerType == targetElementType)
        {
            inLaserAttack = true;
            CounterAttackTurn(enemy.GetCancellationTokenOnDestroy()).Forget();
        }

        return damage;
    }

    private async UniTask CounterAttackTurn(System.Threading.CancellationToken cancellationToken) // 공격받으면 자동공격 
    {
        isActive = false;
        while (inLaserAttack)
        {
            LaserDraw();
            laserDuration += Time.deltaTime;
            if (laserDuration >= laserMaxDuration)
            {
                LaserReset();
                laserDuration = 0f;
                inLaserAttack = false;
                isActive = true;
            }
            await UniTask.Yield(cancellationToken);
        }
    }
    private void LaserDraw()
    {
        if (!initialized)
        {
            lineRenderer.enabled = true;
            lineRenderer.startWidth = enemy.transform.localScale.y * 0.3f;
            lineRenderer.endWidth = enemy.transform.localScale.y * 0.3f;
            lineRenderer.positionCount = 2;
            initialized = true;
        }
        lineRenderer.SetPosition(0, enemy.transform.position);
        Vector2 dir = (enemy.target.transform.position - enemy.transform.position).normalized;
        float dis = Vector2.Distance(enemy.transform.position, enemy.target.transform.position);
        RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, dir, dis, targetLayer);
        if (hit.collider != null)
        {
            Vector2 offsetPoint = hit.point + dir * 0.1f;
            lineRenderer.SetPosition(1, offsetPoint);
            var find = hit.collider.GetComponent<IDamageAble>();
            if (find != null)
            {
                float percent = enemy.typeEffectiveness.GetDamagePercent(find.ElementType);
                find.OnDamage(Mathf.Clamp((int)((enemy.atk - find.Defense) * percent), 1, int.MaxValue));
            }
        }
    }
    private void LaserReset()
    {
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
            lineRenderer.positionCount = 0;
        }
        initialized = false;
    }

}
