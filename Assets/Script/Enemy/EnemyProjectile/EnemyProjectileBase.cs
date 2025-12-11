using UnityEngine;

public abstract class EnemyProjectileBase : MonoBehaviour , IMoveAble
{
    [SerializeField]
    private Sprite sprite;
    private SpriteRenderer spriteRenderer;
    protected PoolsId poolsId;
    public PoolsId PoolsId => poolsId;
    protected Enemy enemyData;
    public Enemy Enemy => enemyData;
    protected Transform target;
    protected IDamageAble targetDamageAble;

    protected TypeEffectiveness typeEffectiveness;
    public ElementType ElementType => typeEffectiveness.Type;

    public bool IsStun { get; set; }
    public float BaseSpeed => enemyData.bulletSpeed;
    public float CurrentSpeed { get => currentSpeed; set => currentSpeed = value; }
    protected float currentSpeed = 0f;
    public virtual void Init(Enemy data, TypeEffectiveness typeEffectiveness)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && sprite != null)
        {
            spriteRenderer.sprite = sprite;

        }
        this.enemyData = data;
        this.typeEffectiveness = typeEffectiveness;
        currentSpeed = BaseSpeed;
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

        if (collision.CompareTag("Player") || collision.CompareTag(TagIds.DroneTag))
        {
            HitTarget(collision);
        }
    }

}
