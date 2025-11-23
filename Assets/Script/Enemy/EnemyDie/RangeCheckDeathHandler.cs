using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class RangeCheckDeathHandler : BaseDie
{
    Collider2D[] targetColliders;

#if DEBUG_MODE
    TestRange rangePrefab;
#endif

    protected abstract string[] targets { get; }

    //범위 체크 
    protected void RangeCheck()
    {        
        targetColliders = Physics2D.OverlapCircleAll(enemy.transform.position, enemy.TestRangeRadius, LayerMask.GetMask(targets));
        RangeCheckDelay().Forget();
        if (targetColliders.Length > 0)
        {
            AbilltyToTarget(targetColliders);
        }
    }

    private void AbilltyToTarget(Collider2D[] targetColliders)
    {
        foreach (var collider in targetColliders)
        {
            if (collider.gameObject == enemy.gameObject)
            {
                continue;
            }
#if DEBUG_MODE
#endif            
            DieAbility(collider);
        }
    }

    protected abstract void DieAbility(Collider2D targetCollider);

    public override void Die(Enemy enemy)
    {
        RangeCheck();
        base.Die(enemy);
    }

#if DEBUG_MODE
    private async UniTaskVoid RangeCheckDelay()
    {
        var rangePrefab = Managers.ObjectPoolManager.SpawnObject<TestRange>(PoolsId.TestRange);
        rangePrefab.transform.position = enemy.transform.position;
        var spr = rangePrefab.GetComponent<SpriteRenderer>();
        spr.color = enemy.spriteRenderer.color;
        spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, 0.5f);
        
        float radius = enemy.TestRangeRadius;
        float visualScale = radius * 2f; 
        rangePrefab.transform.localScale = new Vector3(visualScale, visualScale, 1f);
        await UniTask.Delay(1000);
        if(rangePrefab != null)
        {
            Managers.ObjectPoolManager.Despawn(PoolsId.TestRange, rangePrefab.gameObject);
        }
    }
#endif
}