using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class ShadowBursterBullet : Bullet
{
    private float duration = 0.3f;
    private float timer;

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
            for (int i = 0; i < tower.BonusFregmentCount; i++)
            {
                var fregment = Managers.ObjectPoolManager.SpawnObject<FragmentBullet>(PoolsId.FragmentBullet);
                fregment.Init(tower);
                fregment.transform.position = transform.position;
                float radAngle = angle * i * Mathf.Deg2Rad;
                Vector3 dir = new Vector3(Mathf.Cos(radAngle) , Mathf.Sin(radAngle));
                fregment.SetDirWithNoise(dir);
            }
            Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
            timer = 0;
        }
    }
}