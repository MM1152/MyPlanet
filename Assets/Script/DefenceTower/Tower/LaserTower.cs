using UnityEngine;

public class LaserTower : Tower
{
    protected override BaseAttackPrefab CreateAttackPrefab()
    {
        Laser laser = Managers.ObjectPoolManager.SpawnObject<Laser>(PoolsId.Laser);
        //GameObject.Instantiate(attackprefab).GetComponent<Laser>();
        return laser;
    }

    //FIX : 상속받은 자식에서 정의해주기
    public override bool Attack()
    {
        Target = manager.FindTarget()?.transform;
        return base.Attack();
    }
}
