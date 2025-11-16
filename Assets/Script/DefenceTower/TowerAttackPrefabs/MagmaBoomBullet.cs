using UnityEngine;

public class MagmaBoomBullet : ProjectTile
{
    private float duration = 1f;
    private float currentDuration = 0f;

    private int spawnFragmentCount = 6;

    public override void Init(Tower data, TypeEffectiveness typeEffectiveness, IStatusEffect effect)
    {
        base.Init(data, typeEffectiveness, effect);
        poolsId = PoolsId.MagmaBoomBullet;
        currentDuration = duration;
    }

    protected override void HitTarget(Collider2D collision)
    {
        base.HitTarget(collision);
        SpawnFragments();
        Managers.ObjectPoolManager.Despawn(poolsId, gameObject);
    }

    protected override void Update()
    {
        Move();

        currentDuration -= Time.deltaTime;
        if(currentDuration <= 0f)
        {
            SpawnFragments();
            Managers.ObjectPoolManager.Despawn(poolsId, gameObject);
        }
    }

    private void SpawnFragments()
    {
        //1 바라볼 방향 지정
        //1-1 각도 나눠서 쏘기

        float splitAngle = 360f / spawnFragmentCount;

        for(int i = 0; i < spawnFragmentCount; i++)
        {
            FragmentBullet fragmentObj = Managers.ObjectPoolManager.SpawnObject<FragmentBullet>(PoolsId.FragmentBullet);
            fragmentObj.transform.position = transform.position;
            fragmentObj.Init(towerData, typeEffectiveness, effect);
            float angle = splitAngle * i;
            Vector3 dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0f);
            dir.Normalize();
            fragmentObj.SetDir(dir);
        }
    }
}
