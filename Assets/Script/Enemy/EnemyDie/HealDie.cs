using UnityEngine;

public class HealDie : RangeCheckDeathHandler
{
    protected override string[] targets => new string[] { "Enemy" };

    protected override void DieAbility(Collider2D collider)
    {
        var find = collider.GetComponent<Enemy>();
        if (find != null)
        {
            find.OnHeal(enemy.atk);
        }
    }
}

