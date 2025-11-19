using UnityEngine;

public abstract class EnemyProjectileBase : MonoBehaviour
{
    [SerializeField]
    private Sprite sprite;
    private SpriteRenderer spriteRenderer;
    protected PoolsId poolsId;
    protected Enemy enemyData;
    public Enemy Enemy => enemyData;
    protected Transform target;
    protected IDamageAble targetDamageAble;

    protected TypeEffectiveness typeEffectiveness;
    public ElementType ElementType => typeEffectiveness.Type;

    public virtual void Init(Enemy data, TypeEffectiveness typeEffectiveness)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && sprite != null)
        {
            spriteRenderer.sprite = sprite;

        }
        this.enemyData = data;
        this.typeEffectiveness = typeEffectiveness;
    }

    public virtual void SetTarget(Transform target)
    {
        this.target = target;
        targetDamageAble = target?.GetComponent<IDamageAble>();
    }

    protected abstract void HitTarget(Collider2D collision);

    protected abstract void BlockedHit(Collider2D collision);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DefenseTower"))
        {
            BlockedHit(collision);
        }

        if (collision.CompareTag("Player"))
        {
            HitTarget(collision);
        }
    }

}
