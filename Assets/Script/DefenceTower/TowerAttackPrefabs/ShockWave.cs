using UnityEngine;

public class ShockWave : BaseAttackPrefab
{
    public float radius = 2f;
    public float duration = 1f;
    public override void Init(Tower data, TypeEffectiveness typeEffectiveness, IStatusEffect effect)
    {
        base.Init(data, typeEffectiveness, effect);
        duration = 1f;
        transform.localScale = new Vector3(radius, radius, 1f);
        poolsId = PoolsId.ShockWave;
    }

    public override void SetTarget(Transform target, float minNoise, float maxNoise)
    {
        base.SetTarget(target, minNoise, maxNoise);
    }

    private void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0) 
        {
            Managers.ObjectPoolManager.Despawn(poolsId, gameObject);
        }
    }

    protected override void HitTarget(Collider2D collision)
    {
        var find = collision.GetComponent<IDamageAble>();
        if(find != null)
        {
            // 여기서 DeepCopy 로 범위 내 모든 적들이 하나의 effect 효과를 가져서 오류 생김
            find.StatusEffect.Apply(effect.DeepCopy() , find);
        }
    }
}