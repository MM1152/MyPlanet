using UnityEngine;
using Cysharp.Threading.Tasks;
using Unity.Android.Gradle;


public class BossPartner : MonoBehaviour, IDamageAble
{
    [SerializeField] private Enemy enemy; // 부모 보스 참조
    private ElementType elementType => ElementType.Dark; // 보스 파트너의 속성 타입 어둠으로 고정?
    private ElementType targetElementType => ElementType.Light; // 타겟 속성 타입 빛으로 고정
    private ElementType lastAttackerType => targetTypeEffectiveness.Type; // 마지막 공격자 타입 
    private float controlScale = 0.3f; // 보스 파트너 크기 조절용
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
    private LayerMask targetLayer = LayerMask.GetMask("Player"); // 타겟 레이어   
    private TurnSimpleAttack turnSimpleAttack;

    private void Start()
    {
        radius = enemy.GetComponent<CircleCollider2D>().radius; // 부모 반지름 가져오고 궤도를 구해야함 

        // X, Y만 스케일 적용, Z는 원본 유지
        this.transform.localScale = enemy.transform.localScale * controlScale;
        typeEffectiveness.Init(elementType); // 타입 효과 초기화
        angle = Random.Range(0f, 360f); // 랜덤 시작 각도

        if (enemy.attack is BossAttack bossAttack)
        {
            turnSimpleAttack = bossAttack.GetShotStrategy(enemy.ElementType, enemy.enemyData.ID) as TurnSimpleAttack;
            if (turnSimpleAttack == null) return;
            turnSimpleAttack.SetBossPartner(this);
        }
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
        UpdateSelfRotation(); // 자전 
        UpdateOrbitPosition();// 공전
        AttackTurn(); // 보스와 주고받으며 공격
    }

    private void UpdateSelfRotation()
    {
        this.transform.Rotate(0, 0, selfRotaspeed * Time.deltaTime);
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

    private async UniTask CounterAttackTurn() // 공격받으면 자동공격 
    {
        while (inLaserAttack)
        {
            LaserDraw();
            laserDuration += Time.deltaTime;
            if (laserDuration >= laserMaxDuration)
            {
                LaserReset();
                laserDuration = 0f;
                inLaserAttack = false;
            }
            await UniTask.Yield();
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
        lineRenderer.SetPosition(0, this.transform.position);
        Vector2 dir = (enemy.target.transform.position - this.transform.position).normalized;
        float dis = Vector2.Distance(this.transform.position, enemy.target.transform.position);
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, dir, dis, targetLayer);
        if (hit.collider != null)
        {
            Vector2 offsetPoint = hit.point + dir * 0.1f;
            lineRenderer.SetPosition(1, offsetPoint);
            var find = hit.collider.GetComponent<IDamageAble>();
            if (find != null)
            {
                float percent = typeEffectiveness.GetDamagePercent(find.ElementType);
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

    public void OnDamage(int damage) // 보스가 데미지 입을때 같이 데미지 입게
    {
        enemy.OnDamage(damage);
        inLaserAttack = true;

        if (targetTypeEffectiveness.Type == targetElementType)
        {
            CounterAttackTurn().Forget();
        }
    }

    public void OnDead() // 보스가 죽으면 그때 컴포넌트 제외 및 비활성화 필요
    {
    }

    public void Shot()
    {
        var Bullet = CreateProjectile(PoolsId.SimpleBullet);
        Bullet.transform.position = this.transform.position;
        Bullet.Init(enemy, typeEffectiveness);
        Bullet.SetTarget(enemy.target.transform);
    }

    private EnemyProjectileSimple CreateProjectile(PoolsId poolsId)
    {
        var projectileObj = Managers.ObjectPoolManager.SpawnObject<EnemyProjectileSimple>(poolsId);
        EnemyProjectileSimple projectile = projectileObj.GetComponent<EnemyProjectileSimple>();
        return projectile;
    }
}
