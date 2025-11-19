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
        targetColliders = Physics2D.OverlapCircleAll(enemy.transform.position, enemy.enemyData.AttackRange, LayerMask.GetMask(targets));
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
            Debug.Log($"오브젝트 이름 {collider.gameObject.name}");
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
        var rangescale = enemy.enemyData.AttackRange * 2f;
        var col = rangePrefab.GetComponent<SpriteRenderer>().color = enemy.spriteRenderer.color;
        col.a = 0.5f;            

        rangePrefab.transform.localScale = new Vector3(0.35f * rangescale, 0.35f * rangescale, 1f);
        await UniTask.Delay(1000);
        Managers.ObjectPoolManager.Despawn(PoolsId.TestRange, rangePrefab.gameObject);
    }
#endif
}