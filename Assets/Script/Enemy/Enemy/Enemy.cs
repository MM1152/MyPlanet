using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageAble, IMoveAble
{
    private static readonly string TargetTag = "Player";

    public TypeEffectiveness TypeEffectiveness => typeEffectiveness;
    public TypeEffectiveness typeEffectiveness;
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
    public EnemyType enemyType
    {
        get
        {
            if (isBoss)
            {
                return EnemyType.EliteMonster;
            }
            return enemyData.Range > 0 ? EnemyType.Ranged : EnemyType.Melee;
        }
    }

    public float speed;
    public int atk;
    [SerializeField]
    public float attackRange;
    private float baseRange;
    private bool bonusApplied = false;
    public float bulletSpeed => enemyData.Bullet_Speed;
    public float fireInterval => 60f / enemyData.Fire_Rate;
    public float attackInterval;
    public int currentHP;
    public event Action<Enemy> OnDie;
    private AttackManager attackManager;
    private DieManager dieManager;
    private MoveManager moveManager;
    private AbilityManager abilityManager;
    public IAttack attack;
    public BaseDie die;
    public IMove move;
    public BaseAbility ability;
    public float TestRangeRadius;
    public bool isKilledByPlayer { get; private set; }

#if DEBUG_MODE
    public TextSpawnManager textSpawnManager;
#endif
#if DEBUG_MODE
    public SpriteRenderer spriteRenderer { get; private set; }
#endif
    public ZoneSearch zone;
    public Action abilityAction;

    public Action OnBuffRemoved;

    public Action ReturnMoveAction;

    private static readonly HashSet<int> BossIDs = new HashSet<int> { 3026, 4026, 5026, 6026, 7026, 8026 };
    public bool isBoss => BossIDs.Contains(enemyData.ID);

    private void Awake()
    {
        stateMachine = new StateMachine(this);
        spriteRenderer = GetComponent<SpriteRenderer>();
        waveManager = GameObject.FindWithTag(TagIds.WaveManagerTag).GetComponent<WaveManager>();
        textSpawnManager = GameObject.FindWithTag(TagIds.TextUISpawnManagerTag).GetComponent<TextSpawnManager>();
        zone = GetComponentInChildren<ZoneSearch>();
        typeEffectiveness = new TypeEffectiveness();
        dieManager = new DieManager();
        abilityManager = new AbilityManager();
        attackManager = new AttackManager();
        moveManager = new MoveManager();
    }

    public void Initallized(EnemyData.Data data)
    {
        this.enemyData = data;
        currentHP = enemyData.HP;
        atk = enemyData.ATK;
        speed = enemyData.Speed;
        baseRange = enemyData.Range;
        attackRange = baseRange;
#if DEBUG_MODE
        SetColor(enemyData.Attribute);
#endif
        target = GameObject.FindGameObjectWithTag(TargetTag);
        stateMachine.Init(stateMachine.idleState);
        typeEffectiveness.Init(ElementType);
        statusEffect.Init();
        isKilledByPlayer = true;
        IsDead = false;
        attack = attackManager.GetAttack(enemyType);
        die = dieManager.GetDie(enemyData.ID);
        ability = abilityManager.GetAbility(enemyData.ID);
        move = moveManager.GetMove(enemyData.ID);
        move.Init(this);    
        zone?.Init(this);
        ResetActions();
        ability?.SetEnemy(this);   
        ReturnMoveAction = () =>
        {
            if (!IsDead&&enemyType == EnemyType.EliteMonster)
            {
                stateMachine.ChangeState(stateMachine.walkState);
            }
        };     
    }

    private void ResetActions()
    {
        abilityAction = null;
        OnBuffRemoved = null;
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
                spriteRenderer.color = Color.blue;
                break;
            case 3:
                spriteRenderer.color = Color.gray;
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
#endif
            isKilledByPlayer = false;
            SetState(stateMachine.attackState);
        }
        return;
    }
    // 이벤트 활성화 
    private void Update()
    {
        stateMachine.currentState.Execute();

        attackInterval += Time.deltaTime;
        if (ability != null && ability.abilityType == AbilityType.OnUpdate && abilityAction != null && attackInterval >= fireInterval)
        {
            abilityAction?.Invoke();
            attackInterval = 0f;
        }
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
        if (ability != null && ability.abilityType == AbilityType.OnDamage && ability.isActive)
        {
            damage = ability.OnDamage(damage);
        }

        if (damage <= 0) return;

        currentHP -= damage;

#if DEBUG_MODE
        if (damage > 0)
        {
            var text = textSpawnManager.SpawnTextUI(damage.ToString(), this.transform.position);
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
        ReturnMoveAction = null;    
        stateMachine.ChangeState(stateMachine.dieState);
        statusEffect.Clear();
        OnBuffRemoved?.Invoke();
        OnDie?.Invoke(this);
    }

    public void SetBonusRange(int bonus)
    {
        if (bonusApplied) return;

        attackRange = baseRange + bonus;
        bonusApplied = true;
    }

    public void ResetRange()
    {
        attackRange = baseRange;
        bonusApplied = false;
    }
}
