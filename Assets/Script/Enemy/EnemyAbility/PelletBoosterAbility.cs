using UnityEngine;
using System.Collections.Generic;

public class PelletBoosterAbility : BaseAbility
{
    public override AbilityType abilityType => AbilityType.OnUpdate;
    public int boostPellet = 2;

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
            if (targetEnemy == null || targetEnemy.IsDead) continue;

            if (targetEnemy.attack is ShotAttack shotAttack)
            {
                var elementType = targetEnemy.ElementType;

                if (shotAttack.GetShotStrategy(elementType) is SpreadShot spreadShot)
                {
                    spreadShot.SetBonusPellet(boostPellet);
                }
            }
        }
    }

    private void RemoveBonus()
    {
        if (zoneSearch == null) return;

        foreach (var targetEnemy in zoneSearch.enemiesInZone)
        {
            if (targetEnemy == null || targetEnemy.IsDead) continue;

            if (targetEnemy.attack is ShotAttack shotAttack)
            {
                var elementType = targetEnemy.ElementType;
                if (shotAttack.GetShotStrategy(elementType) is SpreadShot spreadShot)
                {
                    spreadShot.ResetPellet();
                }
            }
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
