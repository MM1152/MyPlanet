using UnityEngine;

public class HealDie : RangeCheckDeathHandler
{
    protected override string[] targets => new string[] { "Enemy" };


    private int healPercent;
    
    private int SetHealPercent()
    {
        return enemy.ElementType switch
        {
            ElementType.Ice => DataTableManager.OptionTable.GetValueDataToInt(5032),
            _ => healPercent = 0,
        };
    }
    protected override void DieAbility(Collider2D collider)
    {
        healPercent = SetHealPercent();
        var find = collider.GetComponent<Enemy>();
        if (find != null)
        {
            find.OnHeal(healPercent);
        }
    }
}

