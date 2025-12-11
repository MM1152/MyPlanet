using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TowerFactory : BaseFactory<Tower>
{
    private Dictionary<int, ITowerCreateor> towerCreator = new Dictionary<int, ITowerCreateor>()
    {
        { 2001, new SolarLaserTowerCreator() },
        { 2002, new DarkLaserTowerCreator() },
        { 2003,  new HellFireTowerCreator() },
        { 2004, new FrostRepeaterTowerCreator() },
        { 2005,  new BurstBladeTowerCreator() },
        { 2006, new ShadowBursterTowerCreator() },
        { 2007,  new VolcanoTowerCreator() },   
        { 2008,  new IceRangerTowerCreator() },   
        { 2009,  new LuminaSniperTowerCreator() },   
        { 2010,  new SteelReaperTowerCreator() },
        { 2011,  new MagmaTowerCreator() },
        { 2012,  new FrostBoomberTowerCreator() },
        { 2013,  new IronMineTowerCreator() },
        { 2014,  new BlackMineTowerCreator() },
        { 2015,  new SurgeTowerCreator() },
        { 2016,  new ShadowSurgeTowerCreator() },
        //{ 2017,  new GravityWrapTowerCreator() },
        { 23001,  new GravityFieldTowerCreator() },
        { 23002,  new GravitySinkTowerCreator() },
        { 23003,  new DelayFieldTowerCreator() },
        { 23004,  new AbyssCoreTowerCreator() },
        { 23005,  new DroneTowerCreator() },
        { 23006,  new RepulsorTowerCreator() },
        //{ 7,  new GravityControlTowerCreator() },
        //{ 8,  new ShockWaveTowerCreator() },
    };

    public (int id , Tower tower) GetRandomTower()
    {
        int rand = Random.Range(2001, 2018);
        return (rand , towerCreator[rand].CreateTower());
    }

    public bool ContainTower(int id)
    {
        return towerCreator.ContainsKey(id);
    }

    public override Tower CreateInstance(int id)
    {
        if(!towerCreator.ContainsKey(id))
        {
            return null;
        }
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

public class SolarLaserTowerCreator : ITowerCreateor
{
    public Tower CreateTower()
    {
        var tower = new SolarLaserTower();
        return tower;
    }
}

public class DarkLaserTowerCreator : ITowerCreateor
{
    public Tower CreateTower()
    {
        var tower = new DarkLaserTower();
        return tower;
    }
}

public class FrostRepeaterTowerCreator : ITowerCreateor
{
    public Tower CreateTower()
    {
        var tower = new FrostRepeaterTower();
        return tower;
    }
}

public class BurstBladeTowerCreator : ITowerCreateor
{
    public Tower CreateTower()
    {
        var tower = new BurstBlasterTower();
        return tower;
    }
}

public class ShadowBursterTowerCreator : ITowerCreateor
{
    public Tower CreateTower()
    {
        var tower = new ShadowBusterTower();
        return tower;
    }
}

public class IceRangerTowerCreator : ITowerCreateor
{
    public Tower CreateTower()
    {
        var tower = new IceRangerTower();
        return tower;
    }
}

public class LuminaSniperTowerCreator : ITowerCreateor
{
    public Tower CreateTower()
    {
        var tower = new LuminaSniperTower();
        return tower;
    }
}

public class FrostBoomberTowerCreator : ITowerCreateor
{
    public Tower CreateTower()
    {
        var tower = new FrostBoomberTower();
        return tower;
    }
}

public class SurgeTowerCreator : ITowerCreateor
{
    public Tower CreateTower()
    {
        var tower = new SurgeArresterTower();
        return tower;
    }
}

public class BlackMineTowerCreator : ITowerCreateor
{
    public Tower CreateTower()
    {
        var tower = new BlackMineTower();
        return tower;
    }
}

public class ShadowSurgeTowerCreator : ITowerCreateor
{
    public Tower CreateTower()
    {
        var tower = new ShadowSurgeTower();
        return tower;
    }
}
public class GravityWrapTowerCreator : ITowerCreateor
{
    public Tower CreateTower()
    {
        var tower = new GravityWarpTower();
        return tower;
    }
}

public class GravityFieldTowerCreator : ITowerCreateor
{
    public Tower CreateTower()
    {
        var tower = new GravityFieldTower();
        return tower;
    }
}

public class GravitySinkTowerCreator : ITowerCreateor
{
    public Tower CreateTower()
    {
        var tower = new GravitySinkTower();
        return tower;
    }
}

public class DelayFieldTowerCreator : ITowerCreateor
{
    public Tower CreateTower()
    {
        var tower = new DelayFieldTower();
        return tower;
    }
}

public class AbyssCoreTowerCreator : ITowerCreateor
{
    public Tower CreateTower()
    {
        var tower = new AbyssCoreTower();
        return tower;
    }
}

public class DroneTowerCreator : ITowerCreateor
{
    public Tower CreateTower()
    {
        var tower = new DroneTower();
        return tower;
    }
}

public class RepulsorTowerCreator : ITowerCreateor
{
    public Tower CreateTower()
    {
        var tower = new RepulsorTower();
        return tower;
    }
}