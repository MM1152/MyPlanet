public class HellFireTowerCreator : ITowerCreateor
{
    public Tower CreateTower()
    {
        var hellTower = new HellFireGunTower();
        hellTower.SetLoadAttackPrefab("Bullet");
        return hellTower;
    }
}