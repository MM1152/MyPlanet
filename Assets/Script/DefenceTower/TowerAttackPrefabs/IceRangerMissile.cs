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
        var explosion = Managers.ObjectPoolManager.SpawnObject<Explosion>(PoolsId.Explosion);
        explosion.Init(tower);
        explosion.transform.position = this.transform.position;

        var dir = this.dir;
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float angle = (float)tower.FullNoise / tower.BonusFregmentCount * 0.5f;
        
        var flash = Managers.ObjectPoolManager.SpawnObject<HitParticle>(PoolsId.Flash26bluecrystal);
        flash.transform.position = transform.position + dir * tower.TowerGameObject.transform.localScale.x;

        for (int i = 0; i < tower.BonusFregmentCount; i++)
        {
            var fregment = Managers.ObjectPoolManager.SpawnObject<IceRangerFregment>(PoolsId.IceRangerFregment);
            fregment.Init(tower);
            fregment.SetParticleId(PoolsId.Hit26bluecrystal);

            float radAngle = (angle * i + targetAngle) * Mathf.Deg2Rad;
            Vector3 fregmentDir = new Vector3(Mathf.Cos(radAngle), Mathf.Sin(radAngle), 0f);
            fregment.transform.position = transform.position;
            fregment.transform.rotation = Quaternion.Euler(0f, 0f, angle * i + targetAngle);
            fregment.SetDirNoNoise(fregmentDir.normalized);
        }

        if(gameObject.activeSelf)
            Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
    }
}