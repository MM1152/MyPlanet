using System;
using UnityEngine;
using UnityEngine.Rendering;

public class BasePlanet : MonoBehaviour , IDamageAble
{
    private StatusEffect statusEffect = new StatusEffect();

    public bool IsDead => isDead;
    public ElementType ElementType => elementType;

    public StatusEffect StatusEffect => statusEffect;

    private bool isDead = false;
    private TypeEffectiveness typeEffectiveness = new TypeEffectiveness();

    public event Action OnDieAction;
    public event Action OnRandomOption;
    public event Action OnChangeHp;

    [Header("On Reference In inspector")]
    [SerializeField] private SliderValue slider;

    [Header("Test Datas")]
    public ElementType elementType;
    public int maxHp;
    public int hp;

    private void Awake()
    {
        Init();
        OnChangeHp += OnChanageHP;
    }

    private void Start()
    {
        slider.UpdateSlider(hp, maxHp);
    }

    public virtual void Init()
    {
        typeEffectiveness.Init(elementType);
        hp = maxHp;
    }
        
    public void OnDamage(int damage)
    {
        hp -= damage;
        OnChangeHp?.Invoke();
        if (hp <= 0 && !isDead)
        {
            OnDead();
        }
    }

    public void OnDead()
    {
        isDead = true;
        OnDieAction?.Invoke();

        Destroy(gameObject);
    }

    private void Update()
    {
        OnRandomOption?.Invoke();
    }

    public void OnChanageHP()
    {
        slider.UpdateSlider(hp, maxHp);
    }
    
}
