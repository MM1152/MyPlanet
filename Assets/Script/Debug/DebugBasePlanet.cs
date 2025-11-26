using UnityEngine;

public class DebugBasePlanet : BasePlanet
{
    protected override void Awake() 
    {
        planetData = DataTableManager.PlanetTable.Get(1001);
        typeEffectiveness.Init((ElementType)planetData.Attribute);
        passiveSystem.Init(planetData.Skill_ID);

        maxHp = int.MaxValue;
        hp = (int)(maxHp);
    }
    protected override void Start()
    { }
}
