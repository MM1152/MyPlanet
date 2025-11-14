using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageAble
{       
    private GameObject target;
    private StatusEffect statusEffect = new StatusEffect();
    public GameObject expPrefab;
    public EnemyData.Data enemyData;
    public StateMachine stateMachine;
    public bool IsDead { get; set; }
    
    public ElementType ElementType => (ElementType)enemyData.Attribute;
    public StatusEffect StatusEffect => statusEffect;

    public float speed;
    public int atk;
    public float attackrange;
    private float attackCooldownTimer = 0f;
    [SerializeField] private int currentHP;

    private TypeEffectiveness typeEffectiveness;

    public WaveManager waveManager;

    public event Action<Enemy> OnDie;
    private void Awake()
    {
        stateMachine = new StateMachine(this);
        waveManager = GameObject.FindGameObjectWithTag(TagIds.WaveManager).GetComponent<WaveManager>(); 
    }

    public virtual void Initallized(EnemyData.Data data)
    {
        this.enemyData = data;
        currentHP = enemyData.HP;
        atk = enemyData.ATK;
        speed = enemyData.Speed;
        attackrange = enemyData.AttackRange;

        target = GameObject.FindGameObjectWithTag(TagIds.PlayerTag);
        stateMachine.Init(stateMachine.idleState);
        typeEffectiveness = new TypeEffectiveness();
        typeEffectiveness.Init(ElementType);
        statusEffect.Init();

        IsDead = false;
    }

    // 상태 확인
    private void CheckState()
    {   // 사망 체크
        if( target == null )
        {
            stateMachine.ChangeState(stateMachine.idleState);
            return;
        }

        if (currentHP <= 0)
        {
            stateMachine.ChangeState(stateMachine.dieState);
            return;
        }
        if( target == null)
        {            
            target = GameObject.FindGameObjectWithTag(TagIds.PlayerTag);
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
        if(IsDead) return;
        // 현재 상태 실행
        stateMachine.currentState.Execute();
        // 상태 전환 체크
        CheckState();
    }

    private void LateUpdate()
    {
        statusEffect.Update(Time.deltaTime);
    }

    // 타겟 반환
    public GameObject GetTarget()
    {
        return target;
    }
    // 데미지 처리
    public void OnDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            OnDead();
        }
    }
    // 사망 처리    
    public void OnDead()
    {
        stateMachine.ChangeState(stateMachine.dieState);
        statusEffect.Clear();
        OnDie?.Invoke(this);
    }
}
