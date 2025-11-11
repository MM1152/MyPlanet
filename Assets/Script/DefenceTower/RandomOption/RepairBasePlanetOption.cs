using UnityEngine;

public class RepairBasePlanetOption : RandomOptionBase
{
    private float coolDown = 3f;
    private float currentCoolDown;

    public override string GetOptionStringFormatting()
    {
        return string.Format(optionData.description, baseTowerData.optionValue);
    }

    public override void ResetRandomOption()
    {
        planet.OnPassive -= RepairPlanet;
    }

    public override void SetRandomOption()
    {
        planet.OnPassive += RepairPlanet;
    }

    protected override RandomOptionBase CreateInstance()
    {
        return new RepairBasePlanetOption();
    }

    private void RepairPlanet()
    {
        if (planet.IsDead)
            return;

        currentCoolDown -= Time.deltaTime;

        if(currentCoolDown <= 0)
        {
            int repairAmount = Mathf.CeilToInt(planet.maxHp * 0.01f); // 최대 체력의 1% 회복
            planet.hp = Mathf.Min(planet.hp + repairAmount, planet.maxHp);
            currentCoolDown = coolDown;
            Debug.Log($"행성 회복: {repairAmount}, 현재 체력: {planet.hp}/{planet.maxHp}");
        }

    }       
}