using UnityEngine;
using Cysharp.Threading.Tasks;
public class RainBullet : EnemyProjectileSimple
{
    private Vector3 movedir;

    private float rectYMax;

    private Bounds bulletBounds;

    private float speed;

    private float speedAlpha = 4f;

    public override void Init(Enemy data, TypeEffectiveness typeEffectiveness)
    {
        base.Init(data, typeEffectiveness);
        poolsId = PoolsId.RainBullet;
        bulletBounds = GetComponent<Collider2D>().bounds;   
        speed = enemyData.bulletSpeed * speedAlpha;       
    }

    public void SetRectLind(Enemy enemy)
    {
        rectYMax = Utils.GetScreenBounds().yMax + bulletBounds.extents.y + Vector2.up.y;
    }

    public void SetDirection(Vector3 direction)
    {
        movedir = direction.normalized;
    }

    protected override void HitTarget(Collider2D collision)
    {
        base.HitTarget(collision);
        Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
    }

    protected override void Move()
    {
        transform.position += movedir * speed * Time.deltaTime;
        if (transform.position.y > rectYMax)
        {
            SwitchDirectionAfterDelay().Forget();
        }
    }

    private async UniTaskVoid SwitchDirectionAfterDelay()
    {
        speed = 0;
        await UniTask.Delay(1000, cancellationToken: this.gameObject.GetCancellationTokenOnDestroy());
        if (target == null) return;
        Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
        movedir = dirToTarget;
        speed = enemyData.bulletSpeed;  
    }
}
