using UnityEngine;

public class PlayerKillSplitAbility : BaseAbility
{
    public override AbilityType abilityType => AbilityType.OnUpdate;
    public int splitCount = 1;

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
            if (targetEnemy == null || targetEnemy.IsDead) continue;

            if (targetEnemy.die is SpawnDie spawnDie)
            {
                spawnDie.SetBonusCount(splitCount);
            }
            else if (targetEnemy.die is SplitbornDie splitbornDie)
            {
                splitbornDie.SetBonusCount(splitCount);
            }
        }
    }

    private void RemoveBonus()
    {
        foreach (var targetEnemy in zoneSearch.enemiesInZone)
        {
            if (targetEnemy == null || targetEnemy.IsDead) continue;

            if (targetEnemy.die is SpawnDie spawnDie)
            {
                spawnDie.ResetCount();
            }
            else if (targetEnemy.die is SplitbornDie splitbornDie)
            {
                splitbornDie.ResetCount();
            } 
        }
    }
}
