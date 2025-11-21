using UnityEngine;

public class ExplosionDie : RangeCheckDeathHandler
{
    protected override string[] targets => new string[] { "Player", "Enemy" };

    protected override void DieAbility(Collider2D collider)
    {        
        var find = collider.GetComponent<IDamageAble>();
        if (find != null)
        {            
            float percent = typeEffectiveness.GetDamagePercent(find.ElementType);
            find.OnDamage((int)(enemy.atk * percent));
            #if DEBUG_MODE
            #endif
        }
    }
}
