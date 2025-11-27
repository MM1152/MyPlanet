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
        foreach (var targetEnemy in zoneSearch.enemiesInZone)
        {
            if (targetEnemy == null || targetEnemy.IsDead) continue;

            if (targetEnemy.attack is ShotAttack shotAttack)
            {
                var elementType = targetEnemy.ElementType;
                if (shotAttack.GetShotStrategy(elementType)  is SpreadShot spreadShot)
                {
                    spreadShot.ResetPellet();
                }
            }
        }
    }
}
