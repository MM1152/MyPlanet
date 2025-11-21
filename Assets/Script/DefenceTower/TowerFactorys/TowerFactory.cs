using System.Collections.Generic;
using UnityEditor;

public class TowerFactory : BaseFactory<Tower>
{
    private Dictionary<int, ITowerCreateor> towerCreator = new Dictionary<int, ITowerCreateor>()
    {
        { 2003,  new HellFireTowerCreator() },
        { 2007,  new VolcanoTowerCreator() },
        { 2010,  new SteelReaperTowerCreator() },
        { 2011,  new MagmaTowerCreator() },
        { 2013,  new IronMineTowerCreator() },
        //{ 7,  new GravityControlTowerCreator() },
        //{ 8,  new ShockWaveTowerCreator() },
    };

    public bool ContainTower(int id)
    {
        return towerCreator.ContainsKey(id);
    }

    public override Tower CreateInstance(int id)
    {
        return towerCreator[id].CreateTower();
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

public class GravityControlTowerCreator : ITowerCreateor
{
    public Tower CreateTower()
    {
        var gravityTower = new GravityControlTower();
        return gravityTower;
    }
}

public class ShockWaveTowerCreator : ITowerCreateor
{
    public Tower CreateTower()
    {
        var shockWaveTower = new ShockWaveTower();
        return shockWaveTower;
    }
}