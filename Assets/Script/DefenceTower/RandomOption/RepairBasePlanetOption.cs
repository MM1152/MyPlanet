using UnityEngine;

public class RepairBasePlanetOption : RandomOptionBase
{
    public override void ResetRandomOption()
    {
        planet.OnPassive -= RepairPlanet;
    }

    public override void SetRandomOption()
    {
        planet.OnPassive += RepairPlanet;
    }

    private void RepairPlanet()
    {
        if (planet.IsDead)
            return;

        int repairAmount = Mathf.CeilToInt(planet.maxHp * 0.01f); // 최대 체력의 1% 회복
        planet.hp = Mathf.Min(planet.hp + repairAmount, planet.maxHp);
        Debug.Log($"행성 회복: {repairAmount}, 현재 체력: {planet.hp}/{planet.maxHp}");
    }       
}