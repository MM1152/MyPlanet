using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

public class ProjectTile : BaseAttackPrefab
{
    [SerializeField] private float speed = 5f;

    protected Vector3 dir;
    protected SpriteRenderer spriteRenderer;

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
            Debug.Log($"�� �Ӽ� {find.ElementType}, �� �Ӽ� {(ElementType)towerData.type}, ���� ������ {percent}");
#endif
            find.OnDamage((int)(towerData.damage * percent));
        }
    }
}