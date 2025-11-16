using Cysharp.Threading.Tasks;
using System.Runtime.CompilerServices;
using UnityEngine;

public class IRonMineTower : Tower
{
    private int placeCount = 5;
    public override bool Attack()
    {
        if(attackAble)
        {
            for(int i = 0; i < placeCount; i++)
            {
                Mine attackPrefabs = CreateAttackPrefab() as Mine;
                attackPrefabs.transform.position = tower.transform.position;
                attackPrefabs.Init(this, typeEffectiveness, statusEffect?.DeepCopy());
                attackPrefabs.SetTarget(target, minNoise, maxNoise);
                attackPrefabs.SetDir((tower.transform.position - manager.basePlanet.transform.position).normalized);
            }

            attackAble = false;
            currentAttackInterval = 0;

            return true;
        }

        return false;
    }

    public override void Init(GameObject tower, TowerManager manager, TowerTable.Data data, int slotIndex)
    {
        base.Init(tower, manager, data, slotIndex);
        minNoise = -45f;
        maxNoise = 45f;
    }

    public override void LevelUp()
    {
        base.LevelUp();
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
    }

    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        var mine = Managers.ObjectPoolManager.SpawnObject<Mine>(PoolsId.Mine);
        return mine;
    }
}