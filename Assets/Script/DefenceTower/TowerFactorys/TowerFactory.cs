using System.Collections.Generic;
using UnityEditor;

public class TowerFactory : BaseFactory<Tower>
{
    private Dictionary<int, ITowerCreateor> towerCreator = new Dictionary<int, ITowerCreateor>()
    {
        { 1,  new HellFireTowerCreator() },
        { 2,  new HellFireTowerCreator() },
        { 3,  new HellFireTowerCreator() },
        { 4,  new HellFireTowerCreator() },
        { 5,  new HellFireTowerCreator() },
        { 6,  new HellFireTowerCreator() },
        { 7,  new HellFireTowerCreator() },
        { 8,  new HellFireTowerCreator() },
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

public class MagmaTowerCreator : ITowerCreateor
{
    public Tower CreateTower()
    {
        var magmaTower = new MagmaBoomerTower();
        return magmaTower;
    }
}

public class IronMineTowerCreator : ITowerCreateor
{
    public Tower CreateTower()
    {
        var ironMineTower = new IRonMineTower();
        return ironMineTower;
    }
}