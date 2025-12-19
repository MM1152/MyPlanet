using UnityEngine;
using Cysharp.Threading.Tasks;

public class HomingBullet : EnemyProjectileSimple
{
    private bool isWaiting = true;
    private Vector2 offsetDir;
    public Vector2 OffsetDir => offsetDir;

    public override void Init(Enemy data, TypeEffectiveness typeEffectiveness)
    {
        base.Init(data, typeEffectiveness);  
        poolsId = PoolsId.HomingBullet;
        isWaiting = true;


        AwaitMove().Forget();
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
        if (!isWaiting)
        {
            if (target == null || targetDamageAble.IsDead)
            {
                Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
                return;
            }
            transform.position = Vector2.MoveTowards(transform.position, target.position, currentSpeed * Time.deltaTime);
        }
    }

    private async UniTask AwaitMove()
    {
        offsetDir = (Random.value > 0.5f) ? (Vector2)Enemy.transform.up : -(Vector2)Enemy.transform.up;
        offsetDir.Normalize();
        Vector2 offsetTarget = (Vector2)Enemy.transform.position + offsetDir * Random.Range(0f, 3f);

        while (Vector3.Distance(transform.position, offsetTarget) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, offsetTarget, 1f * Time.deltaTime);
            await UniTask.Yield(this.gameObject.GetCancellationTokenOnDestroy());
        }
        await UniTask.Delay(500, cancellationToken: this.gameObject.GetCancellationTokenOnDestroy());
        isWaiting = false;
    }
}
