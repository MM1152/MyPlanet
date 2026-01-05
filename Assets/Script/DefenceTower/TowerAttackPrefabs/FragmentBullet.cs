using UnityEngine;
using System;

public class FragmentBullet : ProjectTile
{
    private float currentDuration = 0f;
    private PoolsId particleId;
    public override void Init(Tower data)
    {
        base.Init(data);
        poolsId = PoolsId.FragmentBullet;
        currentDuration = tower.BonusFregmentRange / speed;
    }

    protected override void Update()
    {
        Move();
        currentDuration -= Time.deltaTime;

        if (currentDuration <= 0f)
        {
            if(gameObject.activeSelf)
                Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
        }
    }

    public void SetParticleId(PoolsId particleId)
    {
        this.particleId = particleId;
    }

    protected override void HitTarget(Collider2D collision)
    {
        var enemy = collision.GetComponent<Enemy>();
        if (enemy == null && collision.attachedRigidbody != null)
        {
            enemy = collision.attachedRigidbody.GetComponentInParent<Enemy>();
        }
        if (enemy == null) return;

        var barrier = enemy.GetComponentInChildren<Barrier>();
        if (barrier != null && !barrier.IsDead)
        {
            var percent = tower.TypeEffectiveness.GetDamagePercent(barrier.ElementType);
            barrier.OnDamage((int)(tower.CalcurateAttackDamage * percent));
            
            if(gameObject.activeSelf)
            {
                Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
                if(particleId != PoolsId.None)
                {
                    var hitParticle = Managers.ObjectPoolManager.SpawnObject<HitParticle>(particleId);
                    hitParticle.transform.position = collision.ClosestPoint(transform.position);
                }
            }
            return;
        }

        var find = enemy.GetComponent<IDamageAble>();
        if (find != null && find.IsDead) return;
        
        base.HitTarget(collision);
        if(gameObject.activeSelf)
        {
            Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);

            if(particleId != PoolsId.None)
            {
                var hitParticle = Managers.ObjectPoolManager.SpawnObject<HitParticle>(particleId);
                hitParticle.transform.position = collision.ClosestPoint(transform.position);
            }
        }
    }
}
