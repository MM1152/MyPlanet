using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

public class SpreadBullet : EnemyProjectileSimple
{
   private bool isDespawned = false; 
    
   private float range; 

   Vector3 movedir;
    public override void Init(Enemy data, TypeEffectiveness typeEffectiveness)
    {
        base.Init(data, typeEffectiveness);
        poolsId = PoolsId.SpreadBullet;
        isDespawned = false;
        range = data.attackrange;    
        // DelayDespawn().Forget();    
    }
    
    public void SetDirection(Vector3 direction)
    {
        movedir = direction.normalized;
    }   

    protected override void HitTarget(Collider2D collision)
    {
        base.HitTarget(collision);
        isDespawned = true;
        Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);        
    }
    
    protected override void Move()
    {
        if(Vector2.Distance(transform.position,Enemy.transform.position)<range)
        {
            transform.position += movedir * enemyData.bulletSpeed * Time.deltaTime;
        }
        else
        {
            Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
             isDespawned = true;
        }
    }

    // private async UniTask DelayDespawn()
    // {
    //     await UniTask.Delay(500, cancellationToken: this.gameObject.GetCancellationTokenOnDestroy());
    //     if (isDespawned)
    //     {
    //         Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
    //     }
    // }   
}
