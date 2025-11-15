using System.Collections.Generic;
using UnityEditor;

public class TowerFactory : BaseFactory<Tower>
{
    private Dictionary<int, ITowerCreateor> towerCreator = new Dictionary<int, ITowerCreateor>()
    {
        { 1,  new SteelReaperTowerCreator() },
        { 2,  new SteelReaperTowerCreator() },
        { 3,  new SteelReaperTowerCreator() },
        { 4,  new SteelReaperTowerCreator() },
        { 5,  new VolcanoTowerCreator() },
        { 6,  new VolcanoTowerCreator() },
        { 7,  new VolcanoTowerCreator() },
        { 8,  new VolcanoTowerCreator() },
        { 9,  new HellFireTowerCreator() },
        { 10,  new HellFireTowerCreator() },
        { 11,  new HellFireTowerCreator() },
        { 12,  new HellFireTowerCreator() },
    };

    public override Tower CreateInstance(int id)
    {
        return towerCreator[id].CreateTower();
    }
}

public class LaserTowerCreator : ITowerCreateor
{
    public Tower CreateTower()
    {
        var laser = new LaserTower();
        return laser;
    }
}

public class HellFireTowerCreator : ITowerCreateor
{
    public Tower CreateTower()
    {
        var hellTower = new HellFireGunTower();
        return hellTower;
    }
}

public class VolcanoTowerCreator : ITowerCreateor
{
    public Tower CreateTower()
    {
        var volcanoTower = new VolcanoLauncher();
        return volcanoTower;
    }
}

public class SteelReaperTowerCreator : ITowerCreateor
{
    public Tower CreateTower()
    {
        var steelReaperTower = new SteelReaperTower();
        return steelReaperTower;
    }
}