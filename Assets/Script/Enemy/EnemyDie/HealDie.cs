using UnityEngine;

public class HealDie : RangeCheckDeathHandler
{
    protected override string[] targets => new string[] { "Enemy" };

    protected PoolsId particleId => PoolsId.HitHealParticle;

    private int healPercent;

    private bool hasSpawnedParticle = false;
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

        if (!hasSpawnedParticle)
        {
            hasSpawnedParticle = true;
            Debug.Log($"죽은 포지션 {diePosition}");
            HitParticle();
        }
    }

    private void HitParticle()
    {
        Debug.Log("호출");
        var particle = Managers.ObjectPoolManager.SpawnObject<HitParticle>(particleId);
        particle.transform.localScale = particle.transform.localScale * radius;
        particle.transform.position = diePosition;
        Debug.Log($"Explosion Particle Spawned at {diePosition} with scale {particle.transform.localScale}");
        Debug.Log($"{particle.transform.position} Heal {healPercent} HP to Enemy");
    }
}

