using UnityEngine;

public class ArcMissileBullet : EnemyProjectileSimple
{
    private Vector3 movedir;
    private float speed;
    private float turnSpeed = 0.3f; 

    public override void Init(Enemy data, TypeEffectiveness typeEffectiveness)
    {
        base.Init(data, typeEffectiveness);
        poolsId = PoolsId.ArcMissileBullet;       
        speed = enemyData.bulletSpeed;  
    }

    public void SetDirection(Vector2 direction)
    {
        movedir = direction.normalized;
    
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void SetTurnSpeed(float speed)
    {
        turnSpeed = speed;
    }
  
    protected override void HitTarget(Collider2D collision)
    {
        base.HitTarget(collision);
       if(gameObject.activeSelf)
        {
            Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);

            if( poolsId != PoolsId.None)
            { 
                var particle = Managers.ObjectPoolManager.SpawnObject<HitParticle>(particleId);
                particle.transform.position = collision.ClosestPoint(transform.position);
                poolsId = PoolsId.None;
            }   
        }
    }   

    protected override void Move()
    {
        if(target == null)
        {
            Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
            return;
        }
        
        Vector2 targetDir = (target.transform.position - transform.position).normalized;
        movedir = Vector2.MoveTowards(movedir, targetDir, turnSpeed  * Time.deltaTime).normalized;
        
        float angle = Mathf.Atan2(movedir.y, movedir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
       
        transform.position += (Vector3)movedir * speed * Time.deltaTime;       
    }
    
}
