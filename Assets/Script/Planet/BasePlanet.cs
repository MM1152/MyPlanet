using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class BasePlanet : MonoBehaviour, IDamageAble
{
    public bool IsDead => isDead;
    public ElementType ElementType => elementType;
    public StatusEffect StatusEffect => statusEffect;
    public TypeEffectiveness TypeEffectiveness => typeEffectiveness;

    public int BonusDEF => bonusDEF;
    public int FullDEF => planetLevelData.DEF + bonusDEF;
    public int FullHp => hp + shield;
    public int ATK => planetLevelData.ATK;

    protected StatusEffect statusEffect = new StatusEffect();
    protected TypeEffectiveness typeEffectiveness = new TypeEffectiveness();
    protected PassiveSystem passiveSystem = new PassiveSystem();
    public PassiveSystem PassiveSystem => passiveSystem;
    protected PlanetTable.Data planetData;
    public PlanetTable.Data PlanetData => planetData;

    private PlanetData.Data userPlanetData;
    private PlanetLevelUpTable.Data planetLevelData;

    private bool isDead = false;

    private int bonusDEF = 0;
    private List<(int value , float duration)> bonusDEFList = new List<(int value, float duration)>();

    [Header("On Reference In inspector")]
    [SerializeField] private SliderValue hpSlider;
    [SerializeField] private SliderValue shieldSlider;
    [SerializeField] private WaveManager waveManager;
    [Header("Test Datas")]
    public ElementType elementType;
    public int maxHp;
    public int hp;
    public int shield;
    private TextSpawnManager textSpawnManager;

    protected virtual void Awake()
    {
        textSpawnManager = GameObject.FindWithTag(TagIds.TextUISpawnManagerTag).GetComponent<TextSpawnManager>(); 
    }

    protected virtual void Start()
    {
        Init();
        OnChanageHP();
    }

    public virtual void Init()
    {
        planetData = DataTableManager.PlanetTable.Get(FirebaseManager.Instance.PresetData.GetGameData().PlanetId);
        userPlanetData = FirebaseManager.Instance.PlanetData.GetOrigin(planetData.ID);
        planetLevelData = userPlanetData.PlanetLevelData;

        typeEffectiveness.Init((ElementType)planetData.Attribute);
        passiveSystem.Init(planetData.Skill_ID);

        maxHp = planetLevelData.HP;
        hp = (int)(maxHp);
    }

    public void RepairHp(int amount)
    {
        if (amount == 0) amount = 1;
        hp += amount;
        hp = Mathf.Clamp(hp, 0, maxHp);
        OnChanageHP();
    }

    public void RepairHpToPercent(float percent)
    {
        hp += (int)(maxHp * percent);
        hp = Mathf.Clamp(hp, 0, maxHp);
        OnChanageHP();
    }

    public void OnDamage(int damage)
    {
        if(shield > 0)
        {
            int damageToShield = shield - damage;
            shield -= damage;
            if(damageToShield < 0)
            {
                shield = 0;
                damage = damageToShield;
            }
        }
        hp -= damage;
        textSpawnManager?.SpawnTextUI(damage.ToString(), transform.position).SetColor(Color.yellow);
        OnChanageHP();
        if (hp <= 0 && !isDead)
        {
            OnDead();
        }
    }

    public void OnDead()
    {
        isDead = true;
        waveManager.EndGame(false);
        Destroy(gameObject);
    }

    private void Update()
    {
        passiveSystem?.Update(Time.deltaTime);
        passiveSystem?.CheckUseAblePassive(null, this, null);

        if(bonusDEFList.Count > 0)
        {
            foreach ( var bonus in bonusDEFList.ToArray())
            {
                float newDuration = bonus.duration - Time.deltaTime;
                if (newDuration <= 0f)
                {
                    bonusDEF -= bonus.value;
                    bonusDEFList.Remove(bonus);
                }
                else
                {
                    int index = bonusDEFList.IndexOf(bonus);
                    bonusDEFList[index] = (bonus.value, newDuration);
                }
            }
        }
    }

    public void AddShield(int amount)
    {
        shield += amount;
        OnChanageHP();
    }

    public void AddShieldToPercent(float percent)
    {
        shield += (int)(maxHp * percent);
        OnChanageHP();
    }

    public void RemoveShieldToPercent(float percent)
    {
        shield -= (int)(maxHp * percent);
        shield = Mathf.Clamp(shield, 0, maxHp);
        OnChanageHP();
    }

    public void OnChanageHP()
    {
        hpSlider?.UpdateSlider(hp, maxHp, $"{((float)FullHp / maxHp) * 100f:F2}", FullHp, maxHp);
        shieldSlider?.UpdateSlider(shield, maxHp, "" , "");
    }

    public void AddBonusDFSPercent(float percent , float duration)
    {
        bonusDEF += (int)(planetData.DEF * percent);
        bonusDEFList.Add(((int)(planetData.DEF * percent), duration));
        Debug.Log($"강화된 DEF : {FullDEF}");
    }

    public void AddBonusDEF(int amount, float duration)
    {
        bonusDEF += amount;
        bonusDEFList.Add((amount, duration));
        Debug.Log($"강화된 DEF : {FullDEF}");
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagIds.EnemyTag))
        {
            if ( collision.GetComponent<Enemy>() is Enemy enemy)
            {
                passiveSystem.CheckUseAblePassive(null, this, enemy);
            }
        }
        else if (collision.CompareTag(TagIds.EnemyProjectileTag))
        {
            if (collision.GetComponent<EnemyProjectileSimple>().Enemy is Enemy enemy)
            {
                passiveSystem.CheckUseAblePassive(null, this, enemy);
            }
        }
    }

}
