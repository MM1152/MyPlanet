using System.Threading;
using UnityEngine;

public class MagmaBoomFregment : FragmentBullet
{
    private float moveTimer = 0f;
    private float coolTimer = 0f;
    public override void Init(Tower data)
    {
        base.Init(data);
        moveTimer = tower.BonusFregmentRange / speed;
        coolTimer = tower.BonusCoolTime;
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
        // 사거리 / 스피드 = 시간
        moveTimer -= Time.deltaTime;
        if(moveTimer > 0f)
        {
            Move();
        }
        else if(tower.BonusCoolTime > 0f)
        {
            coolTimer -= Time.deltaTime;
            if(coolTimer <= 0f)
            {
                var explosion = Managers.ObjectPoolManager.SpawnObject<Explosion>(PoolsId.Explosion);
                explosion.transform.position = this.transform.position;
                explosion.Init(tower);
                Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
            }
        }
        else
        {
            var explosion = Managers.ObjectPoolManager.SpawnObject<Explosion>(PoolsId.Explosion);
            explosion.transform.position = this.transform.position;
            explosion.Init(tower);
            Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
        }
    }
}