using UnityEngine;
using System;

public class ShockWave : BaseAttackPrefab
{
    public float duration;
    private TowerTable.UtilTower utilTower;
    private Transform followTarget;

    public float sizeIncreaseValue;
    public float durationTimer;
    public Vector3 targetSize;

    public float FullRange => utilTower.range + tower.BonusAttackRange;
    public override void Init(Tower data)
    {
        base.Init(data);
        utilTower = data.TowerData as TowerTable.UtilTower;

        duration = data.BonusDuration;
        transform.localScale = Vector3.zero;
        durationTimer = duration;
        targetSize = new Vector3(FullRange, FullRange, 1f);
        sizeIncreaseValue = utilTower.range / duration;
        poolsId = PoolsId.ShockWave;
    }

    public void SetFollowTarget(Transform followTarget)
    {
        this.followTarget = followTarget;
    }

    private void Update()
    {
        duration -= Time.deltaTime;

        if (followTarget != null)
        {
            transform.position = followTarget.position;
        }
        
        transform.localScale = new Vector3(
            Mathf.Min(transform.localScale.x + sizeIncreaseValue * Time.deltaTime, targetSize.x),
            Mathf.Min(transform.localScale.y + sizeIncreaseValue * Time.deltaTime, targetSize.y),
            1f);

        if (duration <= 0)
        {
            if (gameObject.activeSelf)
                Managers.ObjectPoolManager.Despawn(poolsId, gameObject);
        }
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            HitTarget(collision);
        }
    }

    protected override void HitTarget(Collider2D collision)
    {
        var enemy = collision.GetComponent<Enemy>();
        if (enemy == null && collision.attachedRigidbody != null)
        {
            enemy = collision.attachedRigidbody.GetComponentInParent<Enemy>();
        }
        if (enemy == null) return;

        var find = enemy.GetComponent<IDamageAble>();
        if (find != null && find.StatusEffect != null)
        {
            // 여기서 DeepCopy 로 범위 내 모든 적들이 하나의 effect 효과를 가져서 오류 생깁
            find.StatusEffect.Apply(new StunStatusEffect(tower.BonusDuration), find);
        }
    }
}