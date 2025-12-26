using UnityEngine;

public class ExplosionDie : RangeCheckDeathHandler
{
    protected override string[] targets => new string[] { "Player" };

    private int explosionAtk;

    protected  PoolsId particleId => PoolsId.HitExplosionParticle;

    private int SetAtk()
    {
        return enemy.ElementType switch
        {
            ElementType.Fire => DataTableManager.OptionTable.GetValueDataToInt(5030),
            _ => explosionAtk = 0,
        };
    }

    protected override void DieAbility(Collider2D collider)
    {
        var find = collider.GetComponent<IDamageAble>();
        explosionAtk = SetAtk();
        if (find != null)
        {
            float percent = typeEffectiveness.GetDamagePercent(find.ElementType);
            find.OnDamage(Mathf.Clamp((int)((explosionAtk - find.Defense) * percent), 1, int.MaxValue));
            Debug.Log($"{enemy.ElementType} Explosion Damage: {Mathf.Clamp((int)((explosionAtk - find.Defense) * percent), 1, int.MaxValue)}");
        }
        HitParticle();
    }

    private void HitParticle()
    {
        var particle = Managers.ObjectPoolManager.SpawnObject<HitParticle>(particleId);
        particle.transform.position = diePosition;
        particle.transform.localScale = particle.transform.localScale * radius;        
        Debug.Log($"Explosion Particle Spawned at {diePosition} with scale {particle.transform.localScale}");
    }
}
