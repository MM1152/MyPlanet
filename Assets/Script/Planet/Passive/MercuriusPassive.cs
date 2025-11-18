public class MercuriusPassive : IPassive
{
    private TowerManager towerManager;
    private BasePlanet basePlanet;

    public void ApplyPassive(PassiveSystem passiveSystem)
    {
        
    }

    public void Init(TowerManager towerManager, BasePlanet basePlanet)
    {
        this.towerManager = towerManager;
        this.basePlanet = basePlanet;
    }
}