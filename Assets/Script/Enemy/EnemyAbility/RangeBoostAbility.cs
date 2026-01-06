using UnityEngine;

public class RangeBoostAbility : BaseAbility
{
    public override AbilityType abilityType => AbilityType.OnUpdate;
    public int boostRange = 1;

    private ZoneSearch zoneSearch;

    public override void SetEnemy(Enemy enemy)
    {
        base.SetEnemy(enemy);
        zoneSearch = Managers.ObjectPoolManager.SpawnObject<ZoneSearch>(PoolsId.Zone);
        zoneSearch.Init(enemy);
        enemy.abilityAction += OnUpdate;
        enemy.OnBuffRemoved += RemoveBonus;
        enemy.OnDie += AbilityDie;
    }

    public override void OnUpdate()
    {
        if (zoneSearch == null) return;

        foreach (var targetEnemy in zoneSearch.enemiesInZone)
        {
            if (targetEnemy == null || targetEnemy.IsDead || targetEnemy.attackRange <= 0) continue;

            targetEnemy.SetBonusRange(boostRange);
        }
    }

    private void RemoveBonus()
    {
        if (zoneSearch == null) return;

        foreach (var targetEnemy in zoneSearch.enemiesInZone)
        {
            if (targetEnemy == null || targetEnemy.IsDead || targetEnemy.attackRange <= 0) continue;

            targetEnemy.ResetRange();

        }
    }
    public void AbilityDie(Enemy enemy)
    {
        if (zoneSearch != null)
        {
            zoneSearch.ZoneDisable();
            Managers.ObjectPoolManager.Despawn(PoolsId.Zone, zoneSearch.gameObject);
            zoneSearch = null;
        }
    }
}
