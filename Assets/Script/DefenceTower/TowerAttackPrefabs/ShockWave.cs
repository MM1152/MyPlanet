using UnityEngine;
using System;

public class ShockWave : BaseAttackPrefab
{
    public float duration;
    private TowerTable.UtilTower utilTower;
    private Transform followTarget;
    public override void Init(Tower data)
    {
        base.Init(data);
        utilTower = data.TowerData as TowerTable.UtilTower;

        duration = data.BonusDuration;
        transform.localScale = new Vector3(utilTower.range, utilTower.range, 1f);
        poolsId = PoolsId.ShockWave;
    }
    
    public void SetFollowTarget(Transform followTarget)
    {
        this.followTarget = followTarget;
    }

    private void Update()
    {
        duration -= Time.deltaTime;

        transform.position = followTarget.position;
        if (duration <= 0) 
        {
            if (gameObject.activeSelf)
                Managers.ObjectPoolManager.Despawn(poolsId, gameObject);
        }
    }

    protected override void HitTarget(Collider2D collision)
    {
        var find = collision.GetComponent<IDamageAble>();
        if(find != null)
        {
            // 여기서 DeepCopy 로 범위 내 모든 적들이 하나의 effect 효과를 가져서 오류 생김
            find.StatusEffect.Apply(new StunStatusEffect(tower.BonusDuration) , find);
        }
    }
}