using UnityEngine;

public class IceRangerMissile : Missile
{
    public override void Init(Tower data)
    {
        base.Init(data);
        poolsId = PoolsId.IceRangerMissile;
    }
    protected override void HitTarget(Collider2D collision)
    {
        base.HitTarget(collision);

        var dir = this.dir;
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float angle = (float)tower.FullNoise / tower.BonusFregmentCount * 0.5f;
        
        for(int i = 0; i < tower.BonusFregmentCount; i++)
        {
            var fregment = Managers.ObjectPoolManager.SpawnObject<FragmentBullet>(PoolsId.FragmentBullet);
            fregment.Init(tower);
            
            float radAngle = (angle * i + targetAngle) * Mathf.Deg2Rad;
            Vector3 fregmentDir = new Vector3(Mathf.Cos(radAngle), Mathf.Sin(radAngle), 0f);
            fregment.transform.position = transform.position;
            fregment.SetDirNoNoise(fregmentDir.normalized);
        }

        Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
    }
}