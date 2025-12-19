using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
public class CounterLaserAbility : BaseAbility
{
    public override AbilityType abilityType => AbilityType.OnDamage;
    public override bool isActive { get; set; }
    private bool initialized = false;
    private LayerMask targetLayer; // 타겟 레
    private bool inLaserAttack = false; // 레이저 공격 중인지 여부
    private bool isWaiting = false; // 다음 공격까지의 대기상태여부 
    private float laserDealy = 2f; // 다음 공격까지 대기시간
    private LineRenderer lineRenderer;
    private float laserDuration = 0f;
    private float laserMaxDuration = 2f;
    private ElementType targetElementType => ElementType.Dark; // 타겟 속성 타입 어둠으로 고정 테이블 
    private Vector2 startPoint;
    private ParticleSystem hitParticle;
    private ParticleSystem flashParticle;


    public override void SetEnemy(Enemy enemy)
    {
        base.SetEnemy(enemy);
        targetLayer = LayerMask.GetMask("Player"); // 타겟 레이어
        lineRenderer = enemy.enemyLineRenderer;
        lineRenderer.enabled = false;
        isActive = true;
        enemy.OnDie += LaserReset;
    }

    public override int OnDamage(int damage)
    {
        if (inLaserAttack) return damage;

        if (enemy.LastAttackerType == targetElementType&&!isWaiting)
        {
            inLaserAttack = true;
            CounterAttackTurn(enemy.GetCancellationTokenOnDestroy()).Forget();
        }
        return damage;
    }

    private async UniTask CounterAttackTurn(System.Threading.CancellationToken cancellationToken) // 공격받으면 자동공격 
    {
        isActive = false;
      Debug.Log($"지속시간 {laserMaxDuration}초 레이저 발사");
        while (inLaserAttack)
        {
            LaserDraw();
            laserDuration += Time.deltaTime;
            if (laserDuration >= laserMaxDuration)
            {
                Debug.Log($"{laserDuration}");
                LaserReset(enemy);
                laserDuration = 0f;
                inLaserAttack = false;
                isActive = true;
                isWaiting = true;
                WaitNextAttack(enemy.GetCancellationTokenOnDestroy()).Forget();
            }
            await UniTask.Yield(cancellationToken);
        }
    }

    private async UniTask WaitNextAttack(System.Threading.CancellationToken cancellationToken)
    {
        Debug.Log("행성 대기시간");
        await UniTask.Delay(TimeSpan.FromSeconds(laserDealy), cancellationToken: cancellationToken);
        isWaiting = false;
    }

    private void LaserDraw()
    {
        if(enemy == null || lineRenderer == null) return;

        if (!initialized)
        {
            lineRenderer.enabled = true;
            lineRenderer.startWidth = enemy.transform.localScale.y * 0.3f;
            lineRenderer.endWidth = enemy.transform.localScale.y * 0.3f;
            lineRenderer.positionCount = 2;
            initialized = true;
        }
        Vector2 dir = (enemy.target.transform.position - enemy.transform.position).normalized;
        float dis = Vector2.Distance(enemy.transform.position, enemy.target.transform.position);
        startPoint = enemy.transform.position + (Vector3)dir * (enemy.transform.localScale.x * 0.5f);
        FlashParticle(startPoint, dir, dis);
        lineRenderer.SetPosition(0, startPoint);
        RaycastHit2D hit = Physics2D.Raycast(startPoint, dir, dis, targetLayer);
        if (hit.collider != null)
        {
            HitParticle(hit.point); 
            lineRenderer.SetPosition(1, hit.point);
            var find = hit.collider.GetComponent<IDamageAble>();
            if (find != null)
            {
                float percent = enemy.typeEffectiveness.GetDamagePercent(find.ElementType);
                find.OnDamage(Mathf.Clamp((int)((enemy.atk - find.Defense) * percent), 1, int.MaxValue));
            }
        }
    }
     private void FlashParticle(Vector2 position, Vector2 direction, float dis)
    {
        if (flashParticle == null)
        {
            flashParticle = Managers.ObjectPoolManager.SpawnObject<ParticleSystem>(PoolsId.LaserBeam4RedFlash);

            if (flashParticle == null) return;
            flashParticle.Play();
        }

        if (flashParticle.transform.position == (Vector3)position) return;

        flashParticle.transform.position = position;
        flashParticle.transform.rotation = Quaternion.LookRotation(direction);

        var flashmain = flashParticle.main;
        flashmain.startRotation = dis / flashmain.startSpeed.constant;
    }

    private void HitParticle(Vector2 position)
    {
        if (hitParticle != null)
        {
            if (hitParticle.transform.position == (Vector3)position)
                return;
            hitParticle.transform.position = position;
            return;
        }

        hitParticle = Managers.ObjectPoolManager.SpawnObject<ParticleSystem>(PoolsId.LaserBeam4RedHit);
        if (hitParticle == null) return;
        hitParticle.transform.position = position;
        hitParticle.Play();
    }
    private void LaserReset(Enemy enemy)
    {
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
            lineRenderer.positionCount = 0;
            if (hitParticle != null)
                Managers.ObjectPoolManager.Despawn(PoolsId.LaserBeam4RedHit, hitParticle.gameObject);
            if (flashParticle != null)
                Managers.ObjectPoolManager.Despawn(PoolsId.LaserBeam4RedFlash, flashParticle.gameObject);
            hitParticle = null;
            flashParticle = null;
        }
        initialized = false;
    }
}
