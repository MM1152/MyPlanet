using System;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageAble, IMoveAble
{
    private static readonly string TargetTag = "Player";

    public TypeEffectiveness TypeEffectiveness => typeEffectiveness;
    public TypeEffectiveness typeEffectiveness;
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

            if (EnemyTypes.IsEliteMonster(enemyData.ID))
            {
                return EnemyType.EliteMonster;
            }
            else if (EnemyTypes.IsBossMonster(enemyData.ID))
            {
                return EnemyType.Boss;
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
    private float abilityInterval = 1f;
    private float nextInterval = 0f;
    public int currentHP;
    public event Action<Enemy> OnDie;

    public event Action OnTerraformingValueChanged;
    public IAttack attack;
    public BaseDie die;
    public IMove move;
    public BaseAbility ability;
    public EnemySpawnManager enemySpawnManager;

    // public float TestRangeRadius;
    public bool isKilledByPlayer { get; set; }

#if DEBUG_MODE
    public TextSpawnManager textSpawnManager;
#endif
    public SpriteRenderer spriteRenderer { get; private set; }
    public ZoneSearch zone;
    public Action abilityAction;

    public Action OnBuffRemoved;

    public Action ReturnMoveAction;

    public LineRenderer enemyLineRenderer;

    public CircleCollider2D enemyCollider;
     
    private WaveWindow bossUi;
    public EnemyPredictionPoisition enemyPredictionPoisition = new EnemyPredictionPoisition();
    private void Awake()
    {
        stateMachine = new StateMachine(this);
        spriteRenderer = GetComponent<SpriteRenderer>();
        waveManager = GameObject.FindWithTag(TagIds.WaveManagerTag)?.GetComponent<WaveManager>();
#if DEBUG_MODE
        textSpawnManager = GameObject.FindWithTag(TagIds.TextUISpawnManagerTag)?.GetComponent<TextSpawnManager>();
#endif
        enemySpawnManager = GameObject.FindWithTag(TagIds.EnemySpawnManagerTag)?.GetComponent<EnemySpawnManager>();
        zone = GetComponentInChildren<ZoneSearch>();
        typeEffectiveness = new TypeEffectiveness();
        enemyLineRenderer = GetComponent<LineRenderer>();
        enemyCollider = GetComponent<CircleCollider2D>();
        enemyPredictionPoisition.Init(this);
    }

    public void DebugToolsInit()
    {
        move.Init(this);
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
        attack = AttackManager.GetAttack(enemyType);
        die = DieManager.GetDie(enemyData.ID);
        ability = AbilityManager.GetAbility(enemyData.ID);
        move = MoveManager.GetMove(enemyType);
        zone?.Init(this);
        ResetActions();
        ability?.SetEnemy(this);
        if(enemyLineRenderer != null)
        {
            enemyLineRenderer.enabled = false;
            enemyLineRenderer.positionCount = 0;
        }  
#if DEBUG_MODE

        if (EnemyTypes.IsBossMonster(data.ID))
        {
            this.transform.localScale = new Vector2(2f, 2f);
            bossUi = GameObject.FindGameObjectWithTag(TagIds.WaveWindowTag)?.GetComponent<WaveWindow>();
            bossUi?.ShowBossUI(enemyData.HP);
        }

        if (EnemyTypes.IsEliteMonster(data.ID)) this.transform.localScale = new Vector2(1.3f, 1.3f);
#endif
        ReturnMoveAction = () =>
        {
            if (!IsDead && (EnemyTypes.IsEliteMonster(data.ID) || EnemyTypes.IsBossMonster(data.ID)))
            {

                stateMachine.ChangeState(stateMachine.walkState);
            }
        };

        if (Variable.IsTutorialActive && EnemyTypes.IsEliteMonster(data.ID))
        {
            TutorialManager tutorialManager = GameObject.FindWithTag(TagIds.TutorialManagerTag).GetComponent<TutorialManager>();
            OnDie += (enemy) => tutorialManager.ForceUpdateTutorial();
        }
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
            isKilledByPlayer = false;
            SetState(stateMachine.attackState);
        }
        return;
    }
    // 이벤트 활성화 
    private void Update()
    {

        stateMachine.currentState.Execute();

         if(IsDead) return; 

        if (ability != null && ability.abilityType == AbilityType.OnUpdate && abilityAction != null && Time.time >= nextInterval)
        {
            abilityAction?.Invoke();
            nextInterval = Time.time + abilityInterval;
        }

        if (move is BaseElementalMove elementalMove && elementalMove.currentStrategy is LeftRinghMove)
        {
            attackInterval += Time.deltaTime;
            if (attack is EliteMonsterAttack eliteMonsterAttack && eliteMonsterAttack.GetShotStrategy(ElementType) is TrailShotAttack trailShotAttack && attackInterval >= (fireInterval -(fireInterval * 0.4f)))
            {
                trailShotAttack.ShotLineDraw(this, target);
            }

            if (attack is EliteMonsterAttack rotatingLaserAttack && rotatingLaserAttack.GetShotStrategy(ElementType) is RotatingLaserAttack laserAttack)
            {
                if (attackInterval >= laserAttack.rotationInterval)
                {
                    laserAttack.UpdateLaser(this, target);
                    laserAttack.Shot(this, target);

                    return;
                }
            }
            else if (attackInterval >= fireInterval)
            {
                attack.Attack(this);
            }
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

        if (damage <= 0) damage = 1;

        currentHP -= damage;
        if(bossUi != null)
            bossUi.UpdateBossHP(currentHP, enemyData.HP);

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
            bossUi?.HideBossUI();
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
        if(enemyLineRenderer != null)
        {
            enemyLineRenderer.enabled = false;
            enemyLineRenderer.positionCount = 0;
        }
        IsDead = true;
        this.transform.localScale = new Vector3(0.35f, 0.35f, 1f);
        ReturnMoveAction = null;
        stateMachine.ChangeState(stateMachine.dieState);
        statusEffect.Clear();
        OnBuffRemoved?.Invoke();
        
        if (waveManager != null)
        {
            OnTerraformingValueChanged?.Invoke();
            OnTerraformingValueChanged = null;
        }

        OnDie?.Invoke(this);
        OnDie = null;
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
