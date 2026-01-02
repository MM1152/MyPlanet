using CsvHelper.Configuration.Attributes;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class BossPartner : MonoBehaviour, IDamageAble
{
    private Enemy enemy;
    private ElementType elementType => ElementType.Dark; // 보스 파트너의 속성 타입 어둠으로 고정?
    private ElementType targetElementType => ElementType.Light; // 타겟 속성 타입 빛으로 고정
    private float radius; // Boss 반지름 
    private float radiusPlus = 1f; // 궤도값 구하기 위해 반지름 추가값
    private float orbitRadius => radius + radiusPlus; // 궤도 반지름
    private float roteSpeed = 90f; // 공전속도
    private float speed = 1f; // 각도 증가 속도
    private float selfRotaspeed => 10f; // 자전속도
    private float angle; // 현재 각도
    private EnemyType enemyType => enemy.enemyType; // 보스와 동일한 타입
    public StatusEffect StatusEffect => null; // 효과 미적용 
    public bool IsDead => false; // 죽음 미적용 보스죽을때 같이 죽음
    public ElementType ElementType => elementType;
    private TypeEffectiveness typeEffectiveness = new TypeEffectiveness();
    private TypeEffectiveness targetTypeEffectiveness = new TypeEffectiveness();
    [SerializeField] private LineRenderer lineRenderer;
    private float attackInterval = 0f;
    private float laserDuration = 0f;
    private float laserMaxDuration = 2f;
    private bool inLaserAttack = false;
    private bool isAttackTurn = false;  // 보스와 주고받는 공격 턴인지 여부
    private bool initialized = false; // 초기화 여부
    private bool isWaiting = false; // 다음 공격까지의 대기상태여부
    private float laserDealy = 2f; // 다음 공격까지 대기시간
    private TurnSimpleAttack turnSimpleAttack;
    private LayerMask targetLayer;
    private Vector2 startPoint;
    private ParticleSystem hitParticle;
    private ParticleSystem flashParticle;

    private int laserDamage;

    [SerializeField] CircleCollider2D circleCollider2D;
    public void Init(Enemy enemy)
    {
        this.enemy = enemy;
        this.transform.SetParent(enemy.transform, true);
        var meshFilter = enemy.GetComponentInChildren<MeshFilter>();
        if (meshFilter != null)
        {
            var bounds = meshFilter.sharedMesh.bounds;
            radius = bounds.extents.magnitude * 0.3f;
        }
        targetLayer = LayerMask.GetMask("Player");
        this.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        typeEffectiveness.Init(elementType); // 타입 효과 초기화
        angle = UnityEngine.Random.Range(0f, 360f); // 랜덤 시작 각도
        laserDamage = DataTableManager.OptionTable.GetValueDataToInt(5068); // 데미지 옵션테이블에서 불러오기   
        enemy.OnDie += Die;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        var tower = collision.GetComponent<BaseAttackPrefab>()?.Tower;
        if (tower == null) return;
        targetTypeEffectiveness.Init((ElementType)tower.TowerData.attribute);
    }

    private void Update()
    {
        // UpdateSelfRotation(); // 자전 
        UpdateOrbitPosition();// 공전
        RotateTowardsTarget();
        AttackTurn(); // 보스와 주고받으며 공격
    }

    private void UpdateSelfRotation()
    {
        this.transform.Rotate(0, 0, selfRotaspeed * Time.deltaTime);
    }

    private void RotateTowardsTarget()
    {
        if (enemy.target == null) return;

        var direction = enemy.target.transform.position - this.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void UpdateOrbitPosition()
    {
        angle += roteSpeed * speed * Time.deltaTime;
        float radian = angle * Mathf.Deg2Rad;
        float x = enemy.transform.position.x + orbitRadius * Mathf.Cos(radian);
        float y = enemy.transform.position.y + orbitRadius * Mathf.Sin(radian);
        this.transform.position = new Vector3(x, y, this.transform.position.z);
    }

    private void AttackTurn() //보스와 주고받으며 공격할때 호출용 
    {
        if (enemy.stateMachine.currentState != enemy.stateMachine.attackState)
            return;

        if (turnSimpleAttack == null)
        {
            if(enemy.attack is BossAttack bossAttack)
            {
                 turnSimpleAttack = bossAttack.GetShotStrategy(enemy.ElementType, enemy.enemyData.ID) as TurnSimpleAttack;
            }
            else
            {
                Debug.LogError("BossAttack이 아닙니다.");   
                return;
            }
        }

        if (!isAttackTurn) return;

        attackInterval += Time.deltaTime;
        if (enemy.fireInterval <= attackInterval)
        {
            Shot();
            attackInterval = 0f;
            isAttackTurn = false;
            turnSimpleAttack.OnPartnerAttackComplete();
        }
    }

    public void EnableAttackTurn()
    {
        isAttackTurn = true;
    }

    private async UniTask CounterAttackTurn(System.Threading.CancellationToken cancellationToken) // 공격받으면 자동공격 
    {
        Debug.Log($"지속시간 {laserMaxDuration}초 레이저 발사");
        while (inLaserAttack)
        {
            LaserDraw();
            laserDuration += Time.deltaTime;
            if (laserDuration >= laserMaxDuration)
            {
                Debug.Log($"{laserDuration}");
                LaserReset();
                laserDuration = 0f;
                inLaserAttack = false;
                isWaiting = true;

                WaitNextAttack(enemy.GetCancellationTokenOnDestroy()).Forget();
            }
            await UniTask.Yield(cancellationToken);
        }
    }

    private async UniTask WaitNextAttack(System.Threading.CancellationToken cancellationToken)
    {
        Debug.Log("방어위성 대기시간");
        await UniTask.Delay(TimeSpan.FromSeconds(laserDealy), cancellationToken: cancellationToken);
        isWaiting = false;
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
        Vector2 dir = (enemy.target.transform.position - this.transform.position).normalized;
        float dis = Vector2.Distance(this.transform.position, enemy.target.transform.position);
        float offset = circleCollider2D.radius * transform.lossyScale.x;
        startPoint = (Vector2)transform.position + dir * offset;
        lineRenderer.SetPosition(0, startPoint);
        FlashParticle(startPoint, dir, dis);
        RaycastHit2D hit = Physics2D.Raycast(startPoint, dir, dis, targetLayer);
        if (hit.collider != null)
        {
            lineRenderer.SetPosition(1, hit.point);
            HitParticle(hit.point);
            var find = hit.collider.GetComponent<IDamageAble>();
            if (find != null)
            {
                float percent = typeEffectiveness.GetDamagePercent(find.ElementType);
                find.OnDamage(Mathf.Clamp((int)((laserDamage - find.Defense) * percent), 1, int.MaxValue));
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

    private void LaserReset()
    {
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
            lineRenderer.positionCount = 0;
            Managers.ObjectPoolManager.Despawn(PoolsId.LaserBeam4RedHit, hitParticle.gameObject);
            Managers.ObjectPoolManager.Despawn(PoolsId.LaserBeam4RedFlash, flashParticle.gameObject);
            hitParticle = null;
            flashParticle = null;
        }
        initialized = false;
    }

    public void OnDamage(int damage) // 보스가 데미지 입을때 같이 데미지 입게
    {
        enemy.OnDamage(damage);
        if (inLaserAttack) return;
        if (targetTypeEffectiveness.Type == targetElementType && !isWaiting)
        {
            inLaserAttack = true;
            CounterAttackTurn(enemy.GetCancellationTokenOnDestroy()).Forget();
        }
    }

    public void OnDead()
    {
        if (hitParticle != null)
        {
            Managers.ObjectPoolManager.Despawn(PoolsId.LaserBeam4RedHit, hitParticle.gameObject);
            hitParticle = null;
        }

        if (flashParticle != null)
        {
            Managers.ObjectPoolManager.Despawn(PoolsId.LaserBeam4RedFlash, flashParticle.gameObject);
            flashParticle = null;
        }
        Managers.ObjectPoolManager.Despawn(PoolsId.BossPartner, this.gameObject);
    }

    public void Die(Enemy enemy)
    {
        OnDead();
    }

    public void Shot()
    {
        var Bullet = CreateProjectile(PoolsId.SimpleBullet);
        Bullet.Init(enemy, typeEffectiveness);
        Bullet.SetTarget(enemy.target.transform);
    }

    private SimpleBullet CreateProjectile(PoolsId poolsId)
    {
        var projectileObj = Managers.ObjectPoolManager.SpawnObject<SimpleBullet>(poolsId);
        SimpleBullet projectile = projectileObj.GetComponent<SimpleBullet>();
        if (enemy.target != null)
        {
            var dir = enemy.target.transform.position - enemy.transform.position;

            projectile.SetHitParticle(PoolsId.Hit13redlaser);
            var flash = Managers.ObjectPoolManager.SpawnObject<HitParticle>(PoolsId.Flash13redlaser);
            flash.transform.position = this.transform.position + dir.normalized * this.transform.localScale.x;
            projectile.transform.position = flash.transform.position;
        }

        return projectile;
    }
}
