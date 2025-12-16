using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class ShadowBursterBullet : Bullet
{
    private float timer;
    private PoolsId particleId;
    public override void Init(Tower data)
    {
        base.Init(data);
        timer = 0;
        poolsId = PoolsId.ShadowBursterBullet;
    }

    public override void SetTarget(Transform target, float noise)
    {
        base.SetTarget(target, noise);
    }

    public void SetParticle(PoolsId particleId)
    {
        this.particleId = particleId;
    }

    protected override void HitTarget(Collider2D collision)
    {
        return;
    }

    protected override Vector3 SetDir()
    {
        return base.SetDir();
    }

    protected override void Update()
    {
        base.Update();
        timer += Time.deltaTime;
        if(timer >= duration)
        {
            float angle = 360f / tower.BonusFregmentCount;
            if(particleId != PoolsId.None)
            {
                var flash = Managers.ObjectPoolManager.SpawnObject<HitParticle>(PoolsId.Flash7pink);
                flash.transform.position = transform.position;
            }
            for (int i = 0; i < tower.BonusFregmentCount; i++)
            {
                var fregment = Managers.ObjectPoolManager.SpawnObject<ShadowBursterFragment>(PoolsId.ShadowBursterFragment);
                fregment.Init(tower);
                fregment.SetParticleId(PoolsId.Hit7pink);
                fregment.transform.position = transform.position;
                float radAngle = angle * i * Mathf.Deg2Rad;
                fregment.transform.rotation = Quaternion.Euler(0, 0, angle * i);
                Vector3 dir = new Vector3(Mathf.Cos(radAngle) , Mathf.Sin(radAngle));
                fregment.SetDirWithNoise(dir);
            }
            if (gameObject.activeSelf)
            {
                var hitParticle = Managers.ObjectPoolManager.SpawnObject<HitParticle>(particleId);
                hitParticle.transform.position = transform.position;
                Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
            }
            timer = 0;
        }
    }
}