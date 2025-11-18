public class UranusPassive : IPassive
{
    private TowerManager towerManager;
    private BasePlanet basePlanet;

    public void ApplyPassive(PassiveSystem passiveSystem)
    {
        var iceTowers = towerManager.GetTowerToAttribute(ElementType.Water);
        foreach (var tower in iceTowers)
        {
            tower.SetStatusEffect(new SlowStatusEffect(0.5f , 0.25f));   
        }
    }

    public void Init(TowerManager towerManager, BasePlanet basePlanet)
    {
        this.towerManager = towerManager;
        this.basePlanet = basePlanet;
    }
}