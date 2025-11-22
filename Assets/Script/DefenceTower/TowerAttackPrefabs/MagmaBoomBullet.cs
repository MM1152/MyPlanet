using UnityEngine;
using System;

public class MagmaBoomBullet : ProjectTile
{
    private float duration = 1f;
    private float currentDuration = 0f;

    private int spawnFragmentCount = 6;

    public override void Init(Tower data)
    {
        base.Init(data);
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
        //1 날라 가는 동작
        //1-1 조각 생성해 날림

        float splitAngle = 360f / spawnFragmentCount;

        for(int i = 0; i < spawnFragmentCount; i++)
        {
            FragmentBullet fragmentObj = Managers.ObjectPoolManager.SpawnObject<FragmentBullet>(PoolsId.FragmentBullet);
            fragmentObj.transform.position = transform.position;
            fragmentObj.Init(tower);
            float angle = splitAngle * i;
            Vector3 dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0f);
            dir.Normalize();
            fragmentObj.SetDirWithNoise(dir);
        }
    }
}
