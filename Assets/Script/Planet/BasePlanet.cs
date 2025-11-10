using System;
using UnityEngine;
using UnityEngine.Rendering;

public class BasePlanet : MonoBehaviour , IDamageAble
{
    public bool IsDead => isDead;
    public ElementType ElementType => elementType;
    public event Action OnDieAction;

    private bool isDead = false;
    private TypeEffectiveness typeEffectiveness = new TypeEffectiveness();

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
        Debug.Log($"행성 데미지 입음 {damage}");
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
}
