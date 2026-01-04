using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageAble, IMoveAble
{
    private static readonly string TargetTag = "Player";

    public TypeEffectiveness TypeEffectiveness => typeEffectiveness;
    public TypeEffectiveness typeEffectiveness;
    public ElementType LastAttackerType { get; set; }
    public GameObject target;
    private StatusEffect statusEffect = new StatusEffect();
    private WaveManager waveManager;
    public WaveManager WaveManager => waveManager;
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
    public float attackRange;
    private float baseRange;
    private bool bonusApplied = false;
    public float bulletSpeed => enemyData.Bullet_Speed;
    public float fireInterval => enemyData.Fire_Rate > 0f ? 60f / enemyData.Fire_Rate : 0f;
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
    private EnemySpawnManager enemySpawnManager;
    public EnemySpawnManager EnemySpawnManager => enemySpawnManager;

    // public float TestRangeRadius;
    public bool isKilledByPlayer { get; set; }

#if DEBUG_MODE
    public TextSpawnManager textSpawnManager;
#endif
    // public ZoneSearch zone;
    public Action abilityAction;

    public Action OnBuffRemoved;

    public Action ReturnMoveAction;

    public Action<int> OnBarrierRefill;

    public LineRenderer enemyLineRenderer;

    public CircleCollider2D enemyCollider;

    private WaveWindow bossUi;
    public EnemyPredictionPoisition enemyPredictionPoisition = new EnemyPredictionPoisition();

    private int stageId;
    private bool isPushed;
    private bool isChaos;
    private float chaosDuration;
    private BasePlanet basePlanet;
    private CancellationTokenSource disAbleCtr;
    public EnemyAsset enemyAsset { get; set; }
    public PoolsId enemyAssetPoolId { get; set; }
    public Transform rotObj;
    private BounsSpawnEnemysManager bounsSpawnEnemysManager;
    public BounsSpawnEnemysManager BounsSpawnEnemysManager => bounsSpawnEnemysManager;
    private GameObject spawnManagers;
    private int maxHP;
    public int MaxHp => maxHP;
    private EnemyHpUI enemyHpUI;
    private void Awake()
    {
        waveManager = GameObject.FindWithTag(TagIds.WaveManagerTag)?.GetComponent<WaveManager>();
#if DEBUG_MODE
        textSpawnManager = GameObject.FindWithTag(TagIds.TextUISpawnManagerTag)?.GetComponent<TextSpawnManager>();
#endif
        spawnManagers = GameObject.FindWithTag(TagIds.EnemySpawnManagerTag);
        enemySpawnManager = spawnManagers?.GetComponent<EnemySpawnManager>();
        bounsSpawnEnemysManager = spawnManagers?.GetComponent<BounsSpawnEnemysManager>();
        typeEffectiveness = new TypeEffectiveness();
        enemyPredictionPoisition.Init(this);
        basePlanet = GameObject.FindWithTag(TagIds.PlayerTag).GetComponent<BasePlanet>();
        stageId = FirebaseManager.Instance.PresetData.GetGameData().stageId;
    }

    public void DebugToolsInit()
    {
        move.Init(this);
    }

    public void Initallized(EnemyData.Data data)
    {
        this.enemyData = data;
        ResetActions();
        stateMachine = new StateMachine(this);
        var stageData = DataTableManager.StageInfomationTable.Get(stageId);
        var percent = 1f;
        if (stageData != null)
        {
            percent = stageData.DIFFICULTY_MULTIPLES;
            percent = Mathf.Clamp(percent, 1, float.MaxValue);
        }

        currentHP = (int)(enemyData.HP * percent);
        maxHP = currentHP;
        atk = (int)(enemyData.ATK * percent);
        speed = enemyData.Speed * percent;
        baseRange = enemyData.Range;
        attackRange = baseRange;
        isChaos = false;
        target = GameObject.FindGameObjectWithTag(TargetTag);
        stateMachine.Init(stateMachine.idleState);
        typeEffectiveness.Init(ElementType);
        statusEffect.Init();
        isKilledByPlayer = true;
        IsDead = false;
        attack = AttackManager.GetAttack(enemyType);
        if (attack is BossAttack bossShotAttack)
        {
            var shotStrategy = bossShotAttack.GetShotStrategy(ElementType, enemyData.ID) as TurnSimpleAttack;
            if (shotStrategy != null)
            {
                shotStrategy.Init(this);
            }
            var laserShotStrategy = bossShotAttack.GetShotStrategy(ElementType, enemyData.ID) as RandomLaserAttack;
            if (laserShotStrategy != null)
            {
                laserShotStrategy.Init(this);
            }
        }
        die = DieManager.GetDie(enemyData.ID);
        ability = AbilityManager.GetAbility(enemyData.ID);
        move = MoveManager.GetMove(enemyType);
        ability?.SetEnemy(this);
        bounsSpawnEnemysManager?.Initialize(enemyData.ID, this);

        if (enemyLineRenderer != null)
        {
            enemyLineRenderer.enabled = false;
            enemyLineRenderer.positionCount = 0;
        }


        if (EnemyTypes.IsBossMonster(data.ID))
        {
            this.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            bossUi = GameObject.FindGameObjectWithTag(TagIds.WaveWindowTag)?.GetComponent<WaveWindow>();
            bossUi?.ShowBossUI(currentHP,enemyData.BOSS_NAME);
        }

        if (EnemyTypes.IsEliteMonster(data.ID))
        {
            this.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            enemyHpUI = Managers.ObjectPoolManager.SpawnObject<EnemyHpUI>(PoolsId.EnemyHpUI);
            enemyHpUI.Init(this);
        }

        ReturnMoveAction = () =>
        {
            if (!IsDead && (EnemyTypes.IsEliteMonster(data.ID) || (EnemyTypes.IsBossMonster(data.ID) && data.ID == 3067)))
            {

                stateMachine.ChangeState(stateMachine.walkState);
            }
        };
    }

    private void ResetActions()
    {
        abilityAction = null;
        OnBuffRemoved = null;
        OnBarrierRefill = null;
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
            isKilledByPlayer = false;
            SetState(stateMachine.attackState);
        }
        return;
    }


    // 이벤트 활성화 
    private void Update()
    {
        if (isPushed) return;
        if (isChaos)
        {
            chaosDuration -= Time.deltaTime;
            SetTarget(enemySpawnManager.GetEnemyChaose(transform.position)?.gameObject);

            if (target == null)
            {
                SetTarget(basePlanet.gameObject);
            }

            if (chaosDuration <= 0)
            {
                isChaos = false;
                SetTarget(basePlanet.gameObject);
            }
        }
        stateMachine.currentState.Execute();

        if (IsDead) return;

        if (ability != null && ability.abilityType == AbilityType.OnUpdate && abilityAction != null && Time.time >= nextInterval)
        {
            abilityAction?.Invoke();
            nextInterval = Time.time + abilityInterval;
        }

        if (move is BaseElementalMove elementalMove && elementalMove.currentStrategy is LeftRinghMove)
        {
            attackInterval += Time.deltaTime;
            if (attack is EliteMonsterAttack eliteMonsterAttack && eliteMonsterAttack.GetShotStrategy(ElementType) is TrailShotAttack trailShotAttack && attackInterval >= (fireInterval - (fireInterval * 0.4f)))
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

    public void SetTarget(GameObject target)
    {
        this.target = target;
        move.Init(this);
    }

    public void OnDamage(int damage)
    {
        if (ability != null && ability.abilityType == AbilityType.OnDamage && ability.isActive)
        {
            damage = ability.OnDamage(damage);
        }

        if (damage < 0) return;

        currentHP -= damage;
        
        if (bossUi != null)
            bossUi.UpdateBossHP(currentHP, maxHP);

        int percent = (int)(((float)currentHP / maxHP) * 100f);
        OnBarrierRefill?.Invoke(percent);

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

        var text = textSpawnManager.SpawnTextUI(healAmount.ToString(), this.transform.position);
        text.SetColor(Color.green);

#endif
    }

    public void OnDead()
    {
        if (enemyLineRenderer != null)
        {
            enemyLineRenderer.enabled = false;
            enemyLineRenderer.positionCount = 0;
        }
        isChaos = false;
        IsDead = true;
        this.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
        ReturnMoveAction = null;
        stateMachine.ChangeState(stateMachine.dieState);
        statusEffect.Clear();
        OnBuffRemoved?.Invoke();

        if (waveManager != null)
        {
            OnTerraformingValueChanged?.Invoke();
            OnTerraformingValueChanged = null;
        }

        enemyHpUI?.Release();
        OnBarrierRefill = null;
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

    public void SetChaos(float duration)
    {
        isChaos = true;
        chaosDuration = duration;
    }

    public void PushEnemy(Vector3 dir, float force, float duration)
    {
        isPushed = true;

        if (disAbleCtr != null && !disAbleCtr.Token.IsCancellationRequested)
        {
            disAbleCtr.Cancel();
            disAbleCtr.Dispose();
        }
        disAbleCtr = new CancellationTokenSource();
        PushEnemyAsync(dir, force, duration, disAbleCtr).Forget();
    }

    private async UniTaskVoid PushEnemyAsync(Vector3 dir, float force, float duration, CancellationTokenSource ctr)
    {
        var speed = (dir * force).magnitude / duration;
        try
        {
            while (duration > 0)
            {
                duration -= Time.deltaTime;
                this.transform.position += dir * speed * Time.deltaTime;
                if (ctr.Token.IsCancellationRequested)
                {
                    throw new Exception();
                }
                await UniTask.Yield();
            }
        }
        catch (Exception)
        {
            Debug.Log("Push Cancelled");
        }
        finally
        {
            isPushed = false;
        }

    }

    private void OnDisable()
    {
        if (disAbleCtr != null && !disAbleCtr.Token.IsCancellationRequested)
        {
            disAbleCtr.Cancel();
            disAbleCtr.Dispose();
            disAbleCtr = null;
        }
    }

    private void OnDestroy()
    {
        if (disAbleCtr != null && !disAbleCtr.Token.IsCancellationRequested)
        {
            disAbleCtr.Cancel();
            disAbleCtr.Dispose();
            disAbleCtr = null;
        }
    }
}
