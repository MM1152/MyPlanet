using UnityEngine;

public class ShockWaveBullet : BaseAttackPrefab
{
    private float duration = 0.5f;
    private float speed = 5f;
    private Vector3 dir;

    public override void Init(Tower data, TypeEffectiveness typeEffectiveness, IStatusEffect effect)
    {
        base.Init(data, typeEffectiveness, effect);
        duration = 0.5f;
        poolsId = PoolsId.ShockWaveBullet;
    }

    public override void SetTarget(Transform target, float minNoise, float maxNoise)
    {
        base.SetTarget(target, minNoise, maxNoise);

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
        shockWave.Init(towerData, typeEffectiveness, effect);
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