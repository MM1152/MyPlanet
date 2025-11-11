public class LaserTowerCreator : ITowerCreateor
{
    public Tower CreateTower()
    {
        var laser = new LaserTower();
        laser.SetLoadAttackPrefab("Laser");
        return laser;
    }
}