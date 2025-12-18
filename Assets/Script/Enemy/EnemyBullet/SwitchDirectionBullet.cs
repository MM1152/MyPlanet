using UnityEngine;
using Cysharp.Threading.Tasks;
public class SwitchDirectionBullet : EnemyProjectileSimple
{    
    private Vector3 movedir;    
  
    public override void Init(Enemy data, TypeEffectiveness typeEffectiveness)
    {
        base.Init(data, typeEffectiveness);
        poolsId = PoolsId.SwitchDirectionBullet;       
        SwitchDirectionAfterDelay().Forget();
    }

    public void SetDirection(Vector3 direction)
    {
        movedir = direction.normalized;
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
        transform.position += movedir * enemyData.bulletSpeed * Time.deltaTime;     
    }

    private async UniTaskVoid SwitchDirectionAfterDelay()
    {
        await UniTask.Delay(1000, cancellationToken: this.gameObject.GetCancellationTokenOnDestroy());
        if(target == null) return;                 
        Vector3 dirToTarget = (target.transform.position - transform.position).normalized;  
        movedir = dirToTarget;
    }
}
