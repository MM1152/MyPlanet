using UnityEngine;

public class HealZoneAbility : BaseAbility
{
    public override AbilityType abilityType => AbilityType.OnUpdate;
    public int healAmount = 10;

    private ZoneSearch zoneSearch;
    public override void SetEnemy(Enemy enemy)
    {
        base.SetEnemy(enemy);
        healAmount = enemy.enemyData.ATK;
        zoneSearch = enemy.zone;
        enemy.abilityAction += OnUpdate;
    }

    public override void OnUpdate()
    {
        Debug.Log("HealZoneAbility: Activating heal zone ability.");
        if (zoneSearch == null) return;

        Debug.Log($"HealZoneAbility: Found {zoneSearch.enemiesInZone.Count} enemies in zone.");

        foreach (var targetEnemy in zoneSearch.enemiesInZone)
        {
            if (targetEnemy == null || targetEnemy.IsDead) continue;
   
            targetEnemy.OnHeal(healAmount);
            Debug.Log($"HealZoneAbility: Healed {healAmount} HP to {targetEnemy.name}");
        }
    }
}
