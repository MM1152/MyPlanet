using UnityEngine;
using System;

public class MagmaBoomBullet : ProjectTile
{
    private int spawnFragmentCount;

    public override void Init(Tower data)
    {
        base.Init(data);
        poolsId = PoolsId.MagmaBoomBullet;
        spawnFragmentCount = data.BonusFregmentCount;
    }

    protected override void HitTarget(Collider2D collision)
    {
        base.HitTarget(collision);
        SpawnFragments();
        if (gameObject.activeSelf)
        {
            Managers.ObjectPoolManager.Despawn(poolsId, gameObject);
            var hitParticle = Managers.ObjectPoolManager.SpawnObject<HitParticle>(PoolsId.Hit18novaorange);
            hitParticle.transform.position = collision.ClosestPoint(transform.position);
        }
        var explosion = Managers.ObjectPoolManager.SpawnObject<Explosion>(PoolsId.Explosion);
        explosion.Init(tower);
        explosion.transform.position = transform.position;
    }

    protected override void Update()
    {
        Move();

        duration -= Time.deltaTime;
        if(duration <= 0f)
        {
            SpawnFragments();
            if (gameObject.activeSelf)
            {
                Managers.ObjectPoolManager.Despawn(poolsId, gameObject);
                var hitParticle = Managers.ObjectPoolManager.SpawnObject<HitParticle>(PoolsId.Hit18novaorange);
                hitParticle.transform.position = transform.position;
            }
        }
    }

    private void SpawnFragments()
    {
        //1 날라 가는 동작
        //1-1 조각 생성해 날림

        float splitAngle = 360f / spawnFragmentCount;
        var flash = Managers.ObjectPoolManager.SpawnObject<HitParticle>(PoolsId.Flash23cube1);
        flash.transform.position = transform.position;
        Managers.SoundManager.PlaySFX(AudiosId.Hit_7);
        for (int i = 0; i < spawnFragmentCount; i++)
        {
            FragmentBullet fragmentObj = Managers.ObjectPoolManager.SpawnObject<MagmaBoomFregment>(PoolsId.MagmaBoomFregment);
            fragmentObj.SetParticleId(PoolsId.Hit23cube1);
            fragmentObj.SetHitSound(AudiosId.Hit_7);
            fragmentObj.transform.position = transform.position;
            fragmentObj.Init(tower);
            float angle = splitAngle * i;
            Vector3 dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0f);
            dir.Normalize();
            fragmentObj.SetDirWithNoise(dir);
        }
    }
}
