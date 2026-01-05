using UnityEngine;
using System;

public class SniperBullet : ProjectTile
{
    private PoolsId particleId;
    private Vector3 initScale;
    private void Awake()
    {
        initScale = transform.localScale;
    }

    public override void Init(Tower data)
    {
        base.Init(data);
        poolsId = PoolsId.SniperBullet;
        speed = 15f;

        transform.localScale = new Vector3(initScale.x * data.BonusWidthSize, initScale.y);
    }

    public void SetParticleId(PoolsId particleId)
    {
        this.particleId = particleId;
    }

    protected override void Update() { }

    protected void FixedUpdate()
    {
        transform.position += dir * speed * Time.deltaTime;
        duration -= Time.deltaTime;

        if (duration <= 0)
        {
            if (gameObject.activeSelf)
                Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
        }
    }

    protected override void HitTarget(Collider2D collision)
    {
        var enemy = collision.GetComponent<Enemy>();
        if (enemy == null && collision.attachedRigidbody != null)
        {
            enemy = collision.attachedRigidbody.GetComponentInParent<Enemy>();
        }
        if (enemy == null)
        {
            base.HitTarget(collision);
            return;
        }

        var barrier = enemy.GetComponentInChildren<Barrier>();
        if (barrier != null && !barrier.IsDead)
        {
            var percent = tower.TypeEffectiveness.GetDamagePercent(barrier.ElementType);
            barrier.OnDamage((int)(tower.CalcurateAttackDamage * percent));
            
            var hitParticle = Managers.ObjectPoolManager.SpawnObject<HitParticle>(particleId);
            hitParticle.transform.position = collision.ClosestPoint(transform.position);
            Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
            return;
        }

        base.HitTarget(collision);

        var hitParticle2 = Managers.ObjectPoolManager.SpawnObject<HitParticle>(particleId);
        hitParticle2.transform.position = collision.ClosestPoint(transform.position);
    }
}