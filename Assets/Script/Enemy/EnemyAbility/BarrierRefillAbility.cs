using UnityEngine;

public class BarrierRefillAbility : BaseAbility
{
    public override AbilityType abilityType => AbilityType.OnUpdate;
    public int refillAmount = 10;

    private ZoneSearch zoneSearch;
    public override void SetEnemy(Enemy enemy)
    {
        base.SetEnemy(enemy);
        refillAmount = enemy.enemyData.ATK;
        zoneSearch = enemy.zone;
        enemy.abilityAction += OnUpdate;        
    }

    public override void OnUpdate()
    {
        if (zoneSearch == null) return;

        foreach (var targetEnemy in zoneSearch.enemiesInZone)
        {
            if (targetEnemy == null || targetEnemy.IsDead) continue;

            if (targetEnemy.ability is BarrierAbility barrierAbility&&enemy.ElementType== targetEnemy.ElementType)
            {
                barrierAbility.RefillBarrier(refillAmount);
            }
        }
    }
}
