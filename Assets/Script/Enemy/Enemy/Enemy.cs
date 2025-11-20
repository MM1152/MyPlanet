using Cysharp.Threading.Tasks;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageAble, IMoveAble
{
    private static readonly string TargetTag = "Player";

    public TypeEffectiveness TypeEffectiveness => typeEffectiveness;
    public int FullDamage => atk;

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

    public bool IsStun { get => isStun; set => isStun = value; }
    private bool isStun;

    public float BaseSpeed => enemyData.Speed;
    public float CurrentSpeed { get => speed; set => speed = value; }
    public float speed;
    public int atk;
    public float attackrange;
    public float attackInterval;
    public EnemyType enemyType => (EnemyType)enemyData.Type;
    [SerializeField] private int currentHP;
    public TypeEffectiveness typeEffectiveness;
    public event Action<Enemy> OnDie;
    private AttackManager attackManager;
    private DieManager dieManager;
    public IAttack attack;
    public BaseDie die;

    public float TestRangeRadius;

    public bool isKilledByPlayer { get; private set; }

#if DEBUG_MODE
    private TextSpawnManager textSpawnManager;
#endif
#if DEBUG_MODE
    public SpriteRenderer spriteRenderer { get; private set; }
#endif

    private void Awake()
    {
        stateMachine = new StateMachine(this);
        spriteRenderer = GetComponent<SpriteRenderer>();
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
#if DEBUG_MODE
        SetColor(enemyData.Attribute);
#endif

        target = GameObject.FindGameObjectWithTag(TargetTag);
        stateMachine.Init(stateMachine.idleState);
        typeEffectiveness = new TypeEffectiveness();
        typeEffectiveness.Init(ElementType);
        statusEffect.Init();
        attackManager = new AttackManager(enemyType, out attack);
        isKilledByPlayer = true;
        IsDead = false;
        dieManager = new DieManager(enemyData.ID, out die);
    }
#if DEBUG_MODE
    private void SetColor(int typeEffectiveness)
    {
        switch (typeEffectiveness)
        {
            case 0:
                spriteRenderer.color = Color.white;
                break;
            case 1:
                spriteRenderer.color = Color.red;
                break;
            case 2:
                spriteRenderer.color = Color.gray;
                break;
            case 3:
                spriteRenderer.color = Color.blue;
                break;
            case 4:
                spriteRenderer.color = Color.yellow;
                break;
            case 5:
                spriteRenderer.color = Color.cyan;
                break;
            default:
                spriteRenderer.color = Color.white;
                break;
        }
    }
#endif

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
        damage = damage == 0 ? 1 : damage;
        currentHP -= damage;
#if DEBUG_MODE
        if (damage > 0)
        {
            var text = textSpawnManager.SpawnTextUI(damage.ToString(), this.transform.position);
            // Debug.Log($"Damage taken: {damage}, Current HP: {currentHP}");
            text.SetColor(Color.red);
        }
#endif
        if (currentHP <= 0)
        {
            OnDead();
        }
    }

    public void OnHeal(int heal)
    {
        int healAmount = Mathf.Min(heal, enemyData.HP - currentHP);
        currentHP += healAmount;
#if DEBUG_MODE
        if (healAmount > 0)
        {
            var text = textSpawnManager.SpawnTextUI(healAmount.ToString(), this.transform.position);
            text.SetColor(Color.green);
        }
#endif
    }

    public void OnDead()
    {
        IsDead = true;
        stateMachine.ChangeState(stateMachine.dieState);
        statusEffect.Clear();
        OnDie?.Invoke(this);
    }
}
