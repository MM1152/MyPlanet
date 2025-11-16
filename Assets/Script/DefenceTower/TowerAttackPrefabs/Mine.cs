using UnityEngine;

public class Mine : BaseAttackPrefab
{
    [SerializeField] protected float duration = 2f;

    protected Vector3 dir;
    protected float noise;
    protected float angle;
    protected float speed = 5f;
    protected float moveStopTime;

    private float currentDuration = 0f;

    public override void Init(Tower data, TypeEffectiveness typeEffectiveness, IStatusEffect effect)
    {
        base.Init(data, typeEffectiveness, effect);
        poolsId = PoolsId.Mine;
        moveStopTime = Random.Range(0.2f , 0.4f);
        currentDuration = 0f;
        poolsId = PoolsId.Mine;
    }

    public override void SetTarget(Transform target, float minNoise, float maxNoise)
    {
        base.SetTarget(target, minNoise, maxNoise);
        noise = Random.Range(minNoise , maxNoise);
        // 타워에서 Dir 값으로 target을 넘겨주든, 방향 뽑을라면 실제 공전중인 타워를 넘기든 해야됌
        // 타워에서 dir값을 넘겨주는 형식으로 뽑아내는 걸로
    }

    public virtual void SetDir(Vector3 dir)
    {
        // 넘겨 받은 dir 값은 정규화 된 값이어야 함    
        this.angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle += noise;
        this.dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) , Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
    }

    protected virtual void Update()
    {
        if(moveStopTime >= 0)
        {
            transform.position += dir * speed * Time.deltaTime;
        }

        if(currentDuration >= duration)
        {
            Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
            //Destroy(gameObject);
            return;
        }

        currentDuration += Time.deltaTime;
        moveStopTime -= Time.deltaTime;
    }

    protected override void HitTarget(Collider2D collision)
    {
        var find = collision.GetComponent<IDamageAble>();
        if (find != null)
        {
            float percent = typeEffectiveness.GetDamagePercent(find.ElementType);
            find.OnDamage((int)(towerData.Damage * percent));
            find.StatusEffect.Apply(effect, find);
            Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
        }
    }
}