using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseAttackPrefab : MonoBehaviour
{
    [SerializeField] private Sprite sprite;

    private SpriteRenderer spriteRenderer;

    protected TypeEffectiveness typeEffectiveness;
    protected Transform target;
    protected IDamageAble targetDamageAble;
    protected Tower towerData;

    protected float minNoise;
    protected float maxNoise;

    protected PoolsId poolsId;
    protected IStatusEffect effect;
    public virtual void Init(Tower data , TypeEffectiveness typeEffectiveness , IStatusEffect effect)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && sprite != null)
        {
            spriteRenderer.sprite = sprite;

        }

        this.towerData = data;
        this.typeEffectiveness = typeEffectiveness;
        this.effect = effect;
    }

    public virtual void SetTarget(Transform target , float minNoise , float maxNoise)
    {
        this.target = target;
        this.minNoise = minNoise;
        this.maxNoise = maxNoise;
        targetDamageAble = target.GetComponent<IDamageAble>();
    }

    protected abstract void HitTarget(Collider2D collision);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( collision.CompareTag("Enemy"))
        {
            HitTarget(collision);
        }
    }
}