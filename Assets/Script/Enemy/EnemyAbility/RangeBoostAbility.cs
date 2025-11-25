using UnityEngine;

public class RangeBoostAbility : BaseAbility
{
    public override AbilityType abilityType => AbilityType.OnUpdate;
    public int boostRange = 1;

    private ZoneSearch zoneSearch;

    public override void SetEnemy(Enemy enemy)
    {
        base.SetEnemy(enemy);
        zoneSearch = enemy.zone;
        enemy.abilityAction += OnUpdate;
        enemy.OnBuffRemoved += RemoveBonus;
    }

    public override void OnUpdate()
    {
        if (zoneSearch == null) return;

        foreach (var targetEnemy in zoneSearch.enemiesInZone)
        {
            if (targetEnemy == null || targetEnemy.IsDead||targetEnemy.attackRange <= 0) continue;
                   
            targetEnemy.SetBonusRange(boostRange);
        }
    }

    private void RemoveBonus()
    {
        foreach (var targetEnemy in zoneSearch.enemiesInZone)
        {
            if (targetEnemy == null || targetEnemy.IsDead||targetEnemy.attackRange <= 0) continue;

            targetEnemy.ResetRange();

        }
    }
}
