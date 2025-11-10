using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseAttackPrefab : MonoBehaviour
{
    [SerializeField] private Sprite sprite;

    private SpriteRenderer spriteRenderer;

    protected TypeEffectiveness typeEffectiveness;
    protected Transform target;
    protected TowerTable.Data towerData;

    protected float minNoise;
    protected float maxNoise;

    public virtual void Init(TowerTable.Data data , TypeEffectiveness typeEffectiveness)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && sprite != null)
        {
            spriteRenderer.sprite = sprite;
        }

        this.towerData = data;
        this.typeEffectiveness = typeEffectiveness;
    }

    public virtual void SetTarget(Transform target , float minNoise , float maxNoise)
    {
        this.target = target;
        this.minNoise = minNoise;
        this.maxNoise = maxNoise;
    }

    protected abstract void HitTarget();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( collision.CompareTag("Enemy"))
        {
            HitTarget();
        }
    }
}