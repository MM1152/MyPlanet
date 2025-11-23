using UnityEngine;
using System;

public class GravityControl : BaseAttackPrefab
{
    [SerializeField] private float radius = 6f;

    private float duration = 1f;
    
    public override void Init(Tower data)
    {
        base.Init(data);
        poolsId = PoolsId.GravityControl;
        //2D라 생각하고 작업 중 추후에 바껴야 할 수 있음
        transform.localScale = new Vector3(radius, radius, 1f);
        duration = 1f;
    }

    private void Update()
    {
        duration -= Time.deltaTime;
        if(duration <= 0f)
        {
            Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
        }
    }

    protected override void HitTarget(Collider2D collision)
    {
        var find = collision.GetComponent<IDamageAble>();
        if (find != null)
        {
            find.StatusEffect.Apply(effect , find);
        }
    }
}
