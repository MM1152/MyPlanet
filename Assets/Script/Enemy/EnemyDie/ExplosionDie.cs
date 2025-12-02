using UnityEngine;

public class ExplosionDie : RangeCheckDeathHandler
{
    protected override string[] targets => new string[] { "Player" };

    protected override void DieAbility(Collider2D collider)
    {
        var find = collider.GetComponent<IDamageAble>();
        if (find != null)
        {
            float percent = typeEffectiveness.GetDamagePercent(find.ElementType);
            find.OnDamage(Mathf.Clamp((int)((enemy.atk - find.Defense) * percent), 1, int.MaxValue));
        }
    }
}
