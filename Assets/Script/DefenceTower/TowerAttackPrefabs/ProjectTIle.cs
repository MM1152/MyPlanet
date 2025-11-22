using UnityEngine;

public class ProjectTile : BaseAttackPrefab
{
    [SerializeField] protected float speed = 5f;
    public float FullBulletSpeed => speed + tower.BonusBulletSpeed;

    protected Vector3 dir;
    
    public override void SetTarget(Transform target , float noise)
    {
        base.SetTarget(target , noise);
        dir = SetDir();

        float rad = Mathf.Atan2(dir.y, dir.x);
        transform.rotation = Quaternion.Euler(0f, 0f, rad * Mathf.Rad2Deg);
    }

    protected virtual Vector3 SetDir()
    {
        dir = target.transform.position - transform.position;
        if (noise != 0)
        {
            float currentAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            currentAngle += noise;
            float radAngle = currentAngle * Mathf.Deg2Rad;
            dir = new Vector3(Mathf.Cos(radAngle), Mathf.Sin(radAngle), 0f);
        }
        return dir.normalized;
    }

    public void SetDir(Vector3 dir)
    {
        this.dir = dir + new Vector3(noise, 0f, 0f);
    }
    
    protected virtual void Update()
    {
        if (target == null || targetDamageAble.IsDead)
        {
            Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
            //Destroy(gameObject);
            return;
        }

        Move();
    }

    protected void Move()
    {
        transform.position += dir * FullBulletSpeed * Time.deltaTime;
    }

    protected override void HitTarget(Collider2D collision)
    {
        var find = collision.GetComponent<IDamageAble>();
        if (find != null)
        {
            float percent = typeEffectiveness.GetDamagePercent(find.ElementType);
            find.OnDamage((int)(tower.FullDamage * percent));
            find.StatusEffect.Apply(effect, find);
        }
    }
}