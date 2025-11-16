using UnityEngine;

public class ProjectTile : BaseAttackPrefab
{
    [SerializeField] protected float speed = 5f;

    protected Vector3 dir;
    
    public override void SetTarget(Transform target , float minNoise , float maxNoise)
    {
        base.SetTarget(target , minNoise , maxNoise);
        dir = SetDir();

        float rad = Mathf.Atan2(dir.y, dir.x);
        transform.rotation = Quaternion.Euler(0f, 0f, rad * Mathf.Rad2Deg);
    }

    protected virtual Vector3 SetDir()
    {
        dir = target.transform.position - transform.position;
        float noise = Random.Range(minNoise , maxNoise);
        return dir.normalized + new Vector3(noise , 0f , 0f);
    }

    public void SetDir(Vector3 dir)
    {
        float noise = Random.Range(minNoise , maxNoise);
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
        transform.position += dir * speed * Time.deltaTime;
    }

    protected override void HitTarget(Collider2D collision)
    {
        var find = collision.GetComponent<IDamageAble>();
        if (find != null)
        {
            float percent = typeEffectiveness.GetDamagePercent(find.ElementType);
            find.OnDamage((int)(towerData.Damage * percent));
            find.StatusEffect.Apply(effect, find);
        }
    }
}