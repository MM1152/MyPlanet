using Cysharp.Threading.Tasks;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageAble, IMoveAble
{
    private static readonly string TargetTag = "Player";
    private GameObject target;
    private StatusEffect statusEffect = new StatusEffect();
    private WaveManager waveManager;
    public WaveManager WaveManager => waveManager;
    public GameObject expPrefab;
    public EnemyData.Data enemyData;
    public StateMachine stateMachine;
    public bool IsDead { get; set; }
    public ElementType ElementType => (ElementType)enemyData.Attribute;
    public StatusEffect StatusEffect => statusEffect;
    public bool IsStun { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public float BaseSpeed => speed;
    public float CurrentSpeed { get => speed; set => speed = value; }
    public float speed;
    public int atk;
    public float attackrange;
    public float attackInterval;
    public EnemyType enemyType => (EnemyType)enemyData.Type;
    public EnemyTier enemyTier => (EnemyTier)enemyData.Tier;
    [SerializeField] private int currentHP;
    public TypeEffectiveness typeEffectiveness;
    public event Action<Enemy> OnDie;
    private EnemyAttackKey attackKey;
    private AttackManager attackManager;
    public IAttack attack;

    public bool isKilledByPlayer { get; private set; }

#if DEBUG_MODE
    private TextSpawnManager textSpawnManager;
#endif
    private void Awake()
    {
        stateMachine = new StateMachine(this);
        waveManager = GameObject.FindWithTag(TagIds.WaveManagerTag).GetComponent<WaveManager>();
        textSpawnManager = GameObject.FindWithTag(TagIds.TextUISpawnManagerTag).GetComponent<TextSpawnManager>();
    }

    public virtual void Initallized(EnemyData.Data data)
    {
        this.enemyData = data;
        currentHP = enemyData.HP;
        atk = enemyData.ATK;
        speed = enemyData.Speed;
        attackrange = enemyData.AttackRange;


        target = GameObject.FindGameObjectWithTag(TargetTag);
        stateMachine.Init(stateMachine.idleState);
        typeEffectiveness = new TypeEffectiveness();
        typeEffectiveness.Init(ElementType);
        statusEffect.Init();
        attackKey = new EnemyAttackKey(enemyType, ElementType, enemyTier);
        attackManager = new AttackManager(attackKey, out attack);
        isKilledByPlayer = true;   
        IsDead = false;
    }

    public void SetState(IState newState)
    {
        stateMachine.ChangeState(newState);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!attack.isAttackColliderOn) return;

        if (collision.CompareTag(TargetTag))
        {
#if DEBUG_MODE
            Debug.Log("콜라이더 충돌 들어옴");
#endif
            isKilledByPlayer = false;
            SetState(stateMachine.attackState);
        }
        return;
    }

    private void Update()
    {
        stateMachine.currentState.Execute();
    }

    private void LateUpdate()
    {
        statusEffect.Update(Time.deltaTime);
    }

    public GameObject GetTarget()
    {
        return target;
    }

    public void OnDamage(int damage)
    {
        currentHP -= damage;
        textSpawnManager.SpawnTextUI(damage.ToString(), this.transform.position);
        if (currentHP <= 0)
        {
            OnDead();
        }
    }

    public void OnDead()
    {
        stateMachine.ChangeState(stateMachine.dieState);
        statusEffect.Clear();
        OnDie?.Invoke(this);
    }
}
