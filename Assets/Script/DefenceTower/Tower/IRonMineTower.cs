using Cysharp.Threading.Tasks;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class IRonMineTower : Tower
{
    //private int placeCount = 5;
    
    public override bool Attack(bool useTarget = true)
    {
        //if(attackAble)
        //{
        //    for(int i = 0; i < placeCount; i++)
        //    {
        //        Mine attackPrefabs = CreateAttackPrefab() as Mine;
        //        attackPrefabs.transform.position = tower.transform.position;
        //        attackPrefabs.Init(this);
        //        attackPrefabs.SetTarget(target, noise);
        //        // Mine 의은 tower 와 basePlanet 사이의 방향으로 설정해서 넘겨줘야함
        //        attackPrefabs.SetDir((tower.transform.position - manager.basePlanet.transform.position).normalized);
        //    }

        //    attackAble = false;
        //    currentAttackInterval = 0;

        //    return true;
        //}
        return base.Attack(false);
    }

    public override void Init(GameObject tower, TowerManager manager, TowerTable.Data data, int slotIndex)
    {
        base.Init(tower, manager, data, slotIndex);
        noise = 45f;
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
    }

    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        var mine = Managers.ObjectPoolManager.SpawnObject<IronMine>(PoolsId.IronMine);
        mine.SetDir((tower.transform.position - manager.basePlanet.transform.position).normalized);
        return mine;
    }
}