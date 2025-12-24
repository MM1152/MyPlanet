using UnityEngine;
using System;

public abstract class BaseAttackPrefab : MonoBehaviour
{
    [SerializeField] private Sprite sprite;

    private SpriteRenderer spriteRenderer;

    protected TypeEffectiveness typeEffectiveness;
    protected Transform target;
    protected Enemy enemy;
    protected IDamageAble targetDamageAble;
    protected Tower tower;
    public Tower Tower => tower;

    protected float noise;

    protected PoolsId poolsId;
    protected IStatusEffect effect;
    private BasePlanet basePlaent;
    protected AudiosId hitSoundId;

    private void Start()
    {
        basePlaent = GameObject.FindWithTag(TagIds.PlayerTag)?.GetComponent<BasePlanet>();
        hitSoundId = AudiosId.None;
    }

    public virtual void Init(Tower data)
    {
        spriteRenderer ??= GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && sprite != null)
        {
            spriteRenderer.sprite = sprite;
        }

        this.tower = data;
        this.typeEffectiveness = data.TypeEffectiveness;
        this.effect = data.StatusEffect?.DeepCopy();
    }

    public virtual void SetTarget(Transform target, float noise)
    {
        this.target = target;
        enemy = target?.GetComponent<Enemy>();
        this.noise = UnityEngine.Random.Range(-noise, noise);
        targetDamageAble = target?.GetComponent<IDamageAble>();
    }

    protected abstract void HitTarget(Collider2D collision);

    public void SetHitSound(AudiosId hitSoundId)
    {
        this.hitSoundId = hitSoundId;
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (basePlaent != null && tower.FullDamage != 0)
            {
                var enemy = collision.GetComponent<Enemy>();
                if (enemy == null && collision.attachedRigidbody != null)
                {
                    enemy = collision.attachedRigidbody.GetComponentInParent<Enemy>();
                }
                if (enemy != null)
                {
                    enemy.LastAttackerType = (ElementType)tower.TowerData.attribute;
                    if (!tower.IsHelper)
                    {
                        basePlaent.PassiveSystem.CheckUseAblePassive(tower, null, enemy);
                    }
                }
                HitTarget(collision);
                if (hitSoundId != AudiosId.None)
                {
                    Managers.SoundManager?.PlaySFX(hitSoundId);
                }
            }
        }
    }
}