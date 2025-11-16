using System.Collections.Generic;
using UnityEditor;

public class TowerFactory : BaseFactory<Tower>
{
    private Dictionary<int, ITowerCreateor> towerCreator = new Dictionary<int, ITowerCreateor>()
    {
        { 1,  new MagmaTowerCreator() },
        { 2,  new MagmaTowerCreator() },
        { 3,  new MagmaTowerCreator() },
        { 4,  new MagmaTowerCreator() },
        { 5,  new MagmaTowerCreator() },
        { 6,  new MagmaTowerCreator() },
        { 7,  new MagmaTowerCreator() },
        { 8,  new MagmaTowerCreator() },
        { 9,  new MagmaTowerCreator() },
        { 10,  new MagmaTowerCreator() },
        { 11,  new MagmaTowerCreator() },
        { 12,  new MagmaTowerCreator() },
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