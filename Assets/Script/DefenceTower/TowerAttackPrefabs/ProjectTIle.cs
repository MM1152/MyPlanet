using System.Data;
using UnityEngine;

public class ProjectTile : BaseAttackPrefab
{
    [SerializeField] protected float speed = 5f;
    public float FullBulletSpeed => speed + tower.BonusBulletSpeed;
    protected float duration;
   
    protected Vector3 dir;

    public override void Init(Tower data)
    {
        base.Init(data);
        duration = data.FullAttackRange / FullBulletSpeed;
    }

    public override void SetTarget(Transform target , float noise)
    {
        base.SetTarget(target , noise);
        dir = SetDir();

        float rad = Mathf.Atan2(dir.y, dir.x);
        transform.rotation = Quaternion.Euler(0f, 0f, rad * Mathf.Rad2Deg);
        duration = tower.FullAttackRange / FullBulletSpeed;
    }

    protected virtual Vector3 SetDir()
    {
        if(enemy != null)
        {
            float predictionTime = (target.position - transform.position).magnitude / FullBulletSpeed;
            Vector3 preditionPos = enemy.enemyPredictionPoisition.GetPredictionPosition(predictionTime);
            dir = (preditionPos - transform.position).normalized;
        }
        else
        {
            dir = (target.transform.position - transform.position).normalized;
        }

        if (noise != 0)
        {
            float currentAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            currentAngle += noise;
            float radAngle = currentAngle * Mathf.Deg2Rad;
            dir = new Vector3(Mathf.Cos(radAngle), Mathf.Sin(radAngle), 0f);
        }
        return dir;
    }

    public void SetDirWithNoise(Vector3 dir)
    {
        this.dir = dir + new Vector3(noise, 0f, 0f);
        duration = tower.FullAttackRange / FullBulletSpeed;
    }

    public void SetDirNoNoise(Vector3 dir)
    {
        this.dir = dir;
        transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        duration = tower.FullAttackRange / FullBulletSpeed;
    }
    
    protected virtual void Update()
    {
        Move();
        if (gameObject.activeSelf)
        {
            if (target == null || targetDamageAble.IsDead)
            {
                if (gameObject.activeSelf)
                    Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
                return;
            }
        }
    }

    protected void Move()
    {
        transform.position += dir * FullBulletSpeed * Time.deltaTime;
        duration -= Time.deltaTime;
        if (gameObject.activeSelf)
        {
            if (duration < 0f)
            {
                if (gameObject.activeSelf)
                    Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
                return;
            }
        }
    }

    protected override void HitTarget(Collider2D collision)
    {
        var find = collision.GetComponent<IDamageAble>();
        if (find != null)
        {
            float percent = typeEffectiveness.GetDamagePercent(find.ElementType);
            find.OnDamage((int)(tower.CalcurateAttackDamage * percent));
            find.StatusEffect.Apply(effect, find);
        }
    }
}