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
        base.HitTarget(collision);

        var hitParticle = Managers.ObjectPoolManager.SpawnObject<HitParticle>(particleId);
        hitParticle.transform.position = collision.ClosestPoint(transform.position);
        
        var barrier = collision.GetComponentInParent<Barrier>();
        if(barrier != null && !barrier.IsDead)
        {
            Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
        }
    }
}