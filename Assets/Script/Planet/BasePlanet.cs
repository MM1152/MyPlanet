using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class BasePlanet : MonoBehaviour, IDamageAble
{
    public bool IsDead => isDead;
    public ElementType ElementType => elementType;
    public StatusEffect StatusEffect => statusEffect;

    private StatusEffect statusEffect = new StatusEffect();
    private TypeEffectiveness typeEffectiveness = new TypeEffectiveness();
    private PassiveSystem passiveSystem = new PassiveSystem();
    public PassiveSystem PassiveSystem => passiveSystem;
    private PlanetTable.Data planetData;
    private bool isDead = false;

    [Header("On Reference In inspector")]
    [SerializeField] private SliderValue slider;
    [SerializeField] private TowerManager towerManager;

    [Header("Test Datas")]
    public ElementType elementType;
    public int maxHp;
    public int hp;
    private TextSpawnManager textSpawnManager;

    private void Awake()
    {
        textSpawnManager = GameObject.FindWithTag(TagIds.TextUISpawnManagerTag).GetComponent<TextSpawnManager>(); 
    }

    private void Start()
    {
        Init();
        slider.UpdateSlider(hp, maxHp , hp / maxHp * 100, hp , maxHp);
    }

    public virtual void Init()
    {
        planetData = DataTableManager.PlanetTable.Get(DataTableManager.PresetTable.GetGameData().PlanetId);
        typeEffectiveness.Init((ElementType)planetData.Attribute);
        passiveSystem.Init(planetData.Skill_ID);

        maxHp = planetData.HP;
        hp = (int)(maxHp * 0.3f);
    }

    public void RepairHp(int amount)
    {
        hp += amount;
        hp = Mathf.Clamp(hp, 0, maxHp);
        OnChanageHP();
    }
        
    public void OnDamage(int damage)
    {
        hp -= damage;
        textSpawnManager.SpawnTextUI(damage.ToString(), transform.position).SetColor(Color.yellow);
        OnChanageHP();
        if (hp <= 0 && !isDead)
        {
            OnDead();
        }
    }

    public void OnDead()
    {
        isDead = true;
        Destroy(gameObject);
    }

    private void Update()
    {
        passiveSystem?.Update(Time.deltaTime);
        passiveSystem?.CheckUseAblePassive(null, this, null);
    }

    public void OnChanageHP()
    {
        slider.UpdateSlider(hp, maxHp, hp / maxHp * 100, hp, maxHp);
    }

}
