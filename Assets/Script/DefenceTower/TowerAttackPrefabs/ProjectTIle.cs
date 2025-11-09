using UnityEngine;

public class ProjectTile : BaseAttackPrefab
{
    [SerializeField] private float speed = 5f;

    protected Vector3 dir;
    protected SpriteRenderer spriteRenderer;

    public override void SetTarget(Transform target)
    {
        base.SetTarget(target);
        dir = SetDir();

        float rad = Mathf.Atan2(dir.y, dir.x);
        transform.rotation = Quaternion.Euler(0f, 0f, rad * Mathf.Rad2Deg);
    }

    protected virtual Vector3 SetDir()
    {
        dir = target.position - transform.position;
        return dir.normalized;
    }
    
    protected virtual void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position += dir * speed * Time.deltaTime;
    }

    protected override void HitTarget()
    {
        var find = target.GetComponent<IDamageAble>();
        if (find != null)
        {
            float percent = typeEffectiveness.GetDamagePercent(find.ElementType);
#if DEBUG_MODE
            Debug.Log($"利 加己 {find.ElementType}, 郴 加己 {(ElementType)towerData.type}, 利侩 单固瘤 {percent}");
#endif
            find.OnDamage((int)(towerData.damage * percent));
        }
    }
}