using UnityEngine;

public abstract class RangeCheckDeathHandler : BaseDie
{
    private Collider2D[] targetColliders;
    protected CircleCollider2D enemyCollider => enemy.enemyCollider;
    protected float radius;
    protected abstract string[] targets { get; }
    protected Vector3 diePosition;

    private float SetRadius()
    {
        return enemy.ElementType switch
        {
            ElementType.Fire => DataTableManager.OptionTable.GetValueDataToFloat(5029),
            ElementType.Ice => DataTableManager.OptionTable.GetValueDataToFloat(5031),
            _ => 0f,
        };
    }
    //범위 체크 
    protected void RangeCheck()
    {
        targetColliders = Physics2D.OverlapCircleAll(diePosition, radius, LayerMask.GetMask(targets));
        Debug.Log($"Pos:{diePosition}, Radius:{radius}");
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
            DieAbility(collider);
        }
    }

    protected abstract void DieAbility(Collider2D targetCollider);

    public override void Die(Enemy enemy)
    {
        diePosition = enemy.transform.position;
        radius = enemy.enemyCollider.radius * SetRadius();

        RangeCheck();
        base.Die(enemy);
    }
}

