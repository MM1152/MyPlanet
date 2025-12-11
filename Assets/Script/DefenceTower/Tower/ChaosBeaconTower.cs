public class ChaosBeaconTower : UtilTower
{
    public override bool Attack(bool useTarget = true)
    {
        CreateAttackPrefab();
        return true;
    }

    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        var chaosBeacon = Managers.ObjectPoolManager.SpawnObject<ChaosBeacon>(PoolsId.ChaosBeacon).GetComponent<ChaosBeacon>();
        chaosBeacon.Init(this);
        var dir = (tower.gameObject.transform.position - planet.transform.position).normalized;
        chaosBeacon.transform.position = tower.transform.position + (dir * BonusAttackRange / 2f);
        chaosBeacon.SetDir(dir);

        return chaosBeacon;
    }
}