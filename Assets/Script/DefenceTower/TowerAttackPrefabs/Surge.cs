using UnityEngine;

public class Surge : BaseAttackPrefab
{
    private float duration = 0f;
    private float timer = 0f;
    public override void Init(Tower data)
    {
        base.Init(data);
        poolsId = PoolsId.Surge;

        duration = 0f;
        timer = 0f;

        transform.localScale = new Vector3(tower.FullAttackRange , tower.FullAttackRange , 1f);
    }

    public override void SetTarget(Transform target, float noise)
    {
        base.SetTarget(target, noise);
    }

    private void FixedUpdate()
    {
        // 충돌감지 기반이라 이때 검사해야 정상동작
        if (timer >= 60 / tower.FullAttackSpeed)
        {
            timer = 0f;
        }

        timer += Time.deltaTime;
    }

    private void Update()
    {
        transform.position = tower.TowerGameObject.transform.position;

        duration += Time.deltaTime;

        if(duration >= tower.BonusDuration)
        {
            Managers.ObjectPoolManager.Despawn(poolsId, gameObject);
        }
    }

    protected override void HitTarget(Collider2D collision)
    {
        return;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (timer < 60 / tower.FullAttackSpeed) return;

        if (collision.CompareTag(TagIds.EnemyTag))
        {
            var find = collision.GetComponent<IDamageAble>();
            if (find != null)
            {
                var percent = tower.TypeEffectiveness.GetDamagePercent(find.ElementType);
                find.OnDamage((int)(tower.CalcurateAttackDamage * percent));
            }
        }
    }
}