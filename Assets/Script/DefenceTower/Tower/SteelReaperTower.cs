using UnityEngine;

public class SteelReaperTower : SniperTower
{
    public override bool Attack()
    {
        Target = manager.FindTarget()?.transform;
        return base.Attack();
    }

    public override void LevelUp()
    {
        base.LevelUp();
    }

}