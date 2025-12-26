using UnityEngine;

public class HealDie : RangeCheckDeathHandler
{
    protected override string[] targets => new string[] { "Enemy" };

    protected  PoolsId particleId => PoolsId.HitHealParticle;

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
           HitParticle();
    }

    private void HitParticle()
    {
        var particle = Managers.ObjectPoolManager.SpawnObject<HitParticle>(particleId);     
        particle.transform.position = diePosition;
        particle.transform.localScale = particle.transform.localScale * radius;   
        Debug.Log($"Explosion Particle Spawned at {diePosition} with scale {particle.transform.localScale}");
        Debug.Log($"{particle.transform.position} Heal {healPercent} HP to Enemy" );  
    }
}

