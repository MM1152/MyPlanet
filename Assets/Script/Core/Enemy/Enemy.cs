using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
public class Enemy : MonoBehaviour, IDamageAble
{
    private static readonly string TargetTag = "Player";
    [SerializeField]
    private GameObject target;
    
    public EnemyData.Data enemyData;
    public StateMachine stateMachine;
    public bool IsDead => stateMachine.dieState.isDead;
    public ElementType ElementType => (ElementType)enemyData.Attribute;
    public float speed;
    public int atk;
    public float attackrange;
    private float attackCooldownTimer = 0f;
    private int currentHP;

    private TypeEffectiveness typeEffectiveness;

    private bool init = false;

    public async UniTaskVoid EnemySetting()
    {
        await DataTableManager.WaitForInitalizeAsync();

        enemyData = DataTableManager.Get<EnemyData>(DataTableIds.EnemyTable).GetData(1);
        target = GameObject.FindGameObjectWithTag(TargetTag);
        stateMachine.Init(stateMachine.idleState);
        var EnemyDataTable = new EnemyData();
        enemyData = EnemyDataTable.GetData(1);
    }
    public void OnEnable()
    {
        target = GameObject.FindGameObjectWithTag(TargetTag);
        currentHP = enemyData.HP;
        atk = enemyData.ATK;
        speed = enemyData.Speed;
        attackrange = enemyData.AttackRange;
        typeEffectiveness = new TypeEffectiveness();
        typeEffectiveness.Init(ElementType);

        init = true;
    }

    private void Awake()
    {
        stateMachine = new StateMachine(this);
        stateMachine.Init(stateMachine.idleState);
    }

    private void OnEnable()
    {
        EnemySetting().Forget();
    }


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
        if (!init) return;
        if (IsDead) return;

        // 현재 상태 실행
        stateMachine.currentState.Execute();
        // 상태 전환 체크
        CheckState();


        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("테스트용 죽이기");

            OnDead();        

        }
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
        currentHP = Mathf.Max(currentHP -= Mathf.RoundToInt(damgePercent), 0);

        if (currentHP <= 0)
        {
            OnDead();
        }
    }
    // 사망 처리    
    public void OnDead()
    {
        if (IsDead && stateMachine.currentState == stateMachine.dieState)
        {
            return; // 이미 DieState면 무시
        }

        stateMachine.ChangeState(stateMachine.dieState);
    }
}
