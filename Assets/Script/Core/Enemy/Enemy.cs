using UnityEngine;

public class Enemy : MonoBehaviour, IDamageAble
{
    private static readonly string TargetTag = "Player";
    [SerializeField]
    private GameObject target;
    public EnemyData.Data enemyData;
    public StateMachine stateMachine;
    public bool IsDead => false;
    public ElementType ElementType => (ElementType)enemyData.Attribute;
    public float speed;
    public int atk;

    public float attackrange;
    private float attackCooldownTimer = 0f;
    private int currentHP;

    private TypeEffectiveness typeEffectiveness;

    private void Awake()
    {
        var EnemyDataTable = new EnemyData();
        enemyData = EnemyDataTable.GetData(1);
    }
    public void OnEnable()
    {
        target = GameObject.FindGameObjectWithTag(TargetTag);
    }
    // 스테이터스 설정
    private void Start()
    {
        stateMachine = new StateMachine(this);
        stateMachine.Init(stateMachine.idleState);

        currentHP = enemyData.HP;
        atk = enemyData.ATK;
        speed = enemyData.Speed;
        attackrange = enemyData.AttackRange;
        typeEffectiveness = new TypeEffectiveness();
        typeEffectiveness.Init(ElementType);
    }
    // 타겟 설정
    // 상태 확인
    private void CheckState()
    {   // 사망 체크
        if (currentHP <= 0)
        {
            stateMachine.ChangeState(stateMachine.dieState);
            return;
        }
        // 거리 계산
        var distanceToTarget = Vector3.Distance(transform.position, target.transform.position);


        // 상태 전환 로직 범위 안에 들어와있는 상태에서 공격 쿨타임이 완료되었을 때 공격 상태로 전환
        if (distanceToTarget <= enemyData.AttackRange && enemyData.AttackInterval <= attackCooldownTimer)
        {
            stateMachine.ChangeState(stateMachine.attackState);
            attackCooldownTimer = 0f;         
            return;
        }
        

        // 이동 상태로 전환        
        if (distanceToTarget > enemyData.AttackRange)
        {
            stateMachine.ChangeState(stateMachine.walkState);
        } 
        else
        {
            stateMachine.ChangeState(stateMachine.idleState);
        }

        attackCooldownTimer += Time.deltaTime;
    }

    // 상태 실행
    private void Update()
    {
        // 현재 상태 실행
        stateMachine.currentState.Execute();
        // 상태 전환 체크
        CheckState();  
    }

    // 타겟 반환
    public GameObject GetTarget()
    {
        return target;
    }
    // 데미지 처리
    public void OnDamage(int damage)
    {
        var damgePercent = typeEffectiveness.GetDamagePercent(target.GetComponent<IDamageAble>().ElementType);
        damgePercent *= damage;
        enemyData.HP = Mathf.Min(currentHP -= Mathf.RoundToInt(damgePercent), 0);

        if (enemyData.HP <= 0)
        {
            OnDead();
        }
    }
    // 사망 처리    
    public void OnDead()
    {
        stateMachine.ChangeState(stateMachine.dieState);
    }
}
