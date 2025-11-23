using UnityEngine;
using System;

public class ShockWaveBullet : BaseAttackPrefab
{
    private float duration = 0.5f;
    private float speed = 5f;
    private Vector3 dir;

    public override void Init(Tower data)
    {
        base.Init(data);
        duration = 0.5f;
        poolsId = PoolsId.ShockWaveBullet;
    }

    public override void SetTarget(Transform target, float noise)
    {
        base.SetTarget(target, noise);

        dir = (target.position - transform.position).normalized;
    }

    private void Update()
    {
        transform.position += dir * speed * Time.deltaTime;

        duration -= Time.deltaTime;
        if(duration <= 0f)
        {
            SpawnShockWave();
        }
    }

    private void SpawnShockWave()
    {
        var shockWave = Managers.ObjectPoolManager.SpawnObject<ShockWave>(PoolsId.ShockWave);
        shockWave.transform.position = transform.position;
        shockWave.Init(tower);
        Managers.ObjectPoolManager.Despawn(poolsId, gameObject);
    }

    protected override void HitTarget(Collider2D collision)
    {
        var find = collision.GetComponent<IDamageAble>();
        if (find != null)
        {
            SpawnShockWave();
        }
    }
}