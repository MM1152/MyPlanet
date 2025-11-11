using UnityEngine;

public class VolcanoLauncher : MissileTower
{
    public int attackCount = 5;

    public override void Init(GameObject tower, TowerManager manager, TowerTable.Data data)
    {
        base.Init(tower, manager, data);
        minNoise = -90f;
        maxNoise = 90f;
    }
    public override bool Attack()
    {
        if(attackAble)
        {
            for (int i = 0; i < attackCount; i++)
            {
                if(base.Attack())
                {
                    attackAble = true;
                }else
                {
                    return false;
                }
            }
        }
        attackAble = false;
        return true;
    }

    public override void LevelUp()
    {
        base.LevelUp();
        minNoise -= 20f;
        maxNoise += 20f;
    }
}
