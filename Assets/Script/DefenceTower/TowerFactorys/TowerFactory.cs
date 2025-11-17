using System.Collections.Generic;
using UnityEditor;

public class TowerFactory : BaseFactory<Tower>
{
    private Dictionary<int, ITowerCreateor> towerCreator = new Dictionary<int, ITowerCreateor>()
    {
        { 1,  new ShockWaveTowerCreator() },
        { 2,  new ShockWaveTowerCreator() },
        { 3,  new ShockWaveTowerCreator() },
        { 4,  new ShockWaveTowerCreator() },
        { 5,  new ShockWaveTowerCreator() },
        { 6,  new ShockWaveTowerCreator() },
        { 7,  new ShockWaveTowerCreator() },
        { 8,  new ShockWaveTowerCreator() },
        { 9,  new ShockWaveTowerCreator() },
        { 10,  new ShockWaveTowerCreator() },
        { 11,  new ShockWaveTowerCreator() },
        { 12,  new ShockWaveTowerCreator() },
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