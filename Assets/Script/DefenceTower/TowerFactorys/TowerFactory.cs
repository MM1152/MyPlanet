using System.Collections.Generic;
using UnityEditor;

public class TowerFactory : BaseFactory<Tower>
{
    Dictionary<int, ITowerCreateor> towerCreator = new Dictionary<int, ITowerCreateor>()
    {
        { 1,  new HellFireTowerCreator() },
        { 2,  new VolcanoTowerCreator() },
        { 3,  new LaserTowerCreator() },
        { 4,  new HellFireTowerCreator() },
        { 5,  new VolcanoTowerCreator() },
        { 6,  new LaserTowerCreator() },
        { 7,  new HellFireTowerCreator() },
        { 8,  new VolcanoTowerCreator() },
        { 9,  new LaserTowerCreator() },
        { 10,  new HellFireTowerCreator() },
        { 11,  new VolcanoTowerCreator() },
        { 12,  new LaserTowerCreator() },
    };

    public override Tower CreateInstance(int id)
    {
        return towerCreator[id].CreateTower();
    }
}