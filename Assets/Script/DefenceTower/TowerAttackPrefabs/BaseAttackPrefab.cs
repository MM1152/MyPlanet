using UnityEngine;
using System;

public abstract class BaseAttackPrefab : MonoBehaviour
{
    [SerializeField] private Sprite sprite;

    private SpriteRenderer spriteRenderer;

    protected TypeEffectiveness typeEffectiveness;
    protected Transform target;
    protected IDamageAble targetDamageAble;
    protected Tower tower;

    protected float noise;

    protected PoolsId poolsId;
    protected IStatusEffect effect;

    private BasePlanet basePlaent;

    private void Start()
    {
        basePlaent = GameObject.FindWithTag(TagIds.PlayerTag).GetComponent<BasePlanet>();
    }

    public virtual void     Init(Tower data)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && sprite != null)
        {
            spriteRenderer.sprite = sprite;
        }

        this.tower = data;
        this.typeEffectiveness = data.TypeEffectiveness;
        this.effect = data.StatusEffect?.DeepCopy();
    }

    public virtual void SetTarget(Transform target , float noise)
    {
        this.target = target;
        this.noise = UnityEngine.Random.Range(-noise , noise);
        targetDamageAble = target?.GetComponent<IDamageAble>();
    }

    protected abstract void HitTarget(Collider2D collision);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( collision.CompareTag("Enemy"))
        {
            HitTarget(collision);
            basePlaent.PassiveSystem.CheckUseAblePassive(tower, null, collision.GetComponent<Enemy>());
        }
    }
}