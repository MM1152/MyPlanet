public class VolcanoTowerCreator : ITowerCreateor
{
    public Tower CreateTower()
    {
        var volcanoTower = new VolcanoLauncher();
        volcanoTower.SetLoadAttackPrefab("Missile");
        return volcanoTower;
    }
}