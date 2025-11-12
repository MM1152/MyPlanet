using System;
using UnityEngine;
using UnityEngine.Rendering;

public class BasePlanet : MonoBehaviour , IDamageAble
{
    private StatusEffect statusEffect = new StatusEffect();

    public bool IsDead => isDead;
    public ElementType ElementType => elementType;

    public StatusEffect StatusEffect => statusEffect;
    public event Action OnDieAction;

    private bool isDead = false;
    private TypeEffectiveness typeEffectiveness = new TypeEffectiveness();

    public event Action OnPassive;

    [Header("Test Datas")]
    public ElementType elementType;
    public int maxHp;
    public int hp;

    private void Awake()
    {
        Init();
    }

    public virtual void Init()
    {
        typeEffectiveness.Init(elementType);
        hp = maxHp;
    }
        
    public void OnDamage(int damage)
    {
        hp -= damage;
        if(hp <= 0 && !isDead)
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
        OnPassive?.Invoke();
    }
}
