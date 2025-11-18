public class MarsPassive : IPassive
{
    private TowerManager towerManager;
    private BasePlanet basePlanet;

    public void ApplyPassive(PassiveSystem passiveSystem)
    {
        var towers = towerManager.GetTowerToAttribute(ElementType.Fire);
        foreach(var tower in towers)
        {
            tower.SetStatusEffect(new BurnStatusEffect(5f, 1f , 70));
        }
    }

    public void Init(TowerManager towerManager, BasePlanet basePlanet)
    {
        this.towerManager = towerManager;
        this.basePlanet = basePlanet;
    }
}