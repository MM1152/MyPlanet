using UnityEngine;

public class HealZoneAbility : BaseAbility
{
    public override AbilityType abilityType => AbilityType.OnUpdate;
    public int healAmount => DataTableManager.OptionTable.GetValueDataToInt(5037);

    private ZoneSearch zoneSearch;
    public override void SetEnemy(Enemy enemy)
    {
        base.SetEnemy(enemy);
        zoneSearch = enemy.zone;
        enemy.abilityAction += OnUpdate;
    }

    public override void OnUpdate()
    {
        if (zoneSearch == null) return;

        foreach (var targetEnemy in zoneSearch.enemiesInZone)
        {
            if (targetEnemy == null || targetEnemy.IsDead) continue;
            Debug.Log($"HealZoneAbility Healed {healAmount} HP to {targetEnemy.name}");
            targetEnemy.OnHeal(healAmount);
        }
    }
}
