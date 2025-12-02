using UnityEngine;

public class OneTimeMeleeAttacker : IAttack
{
    public bool isAttackColliderOn => true;
    public void Attack(Enemy enemy)
    {
        var target = enemy.GetTarget();
        var findTarget = target.GetComponent<IDamageAble>();

        float bonus = enemy.typeEffectiveness.GetDamagePercent(findTarget.ElementType);  
        findTarget.OnDamage((int)(enemy.atk*bonus));              
        enemy.OnDead();
    }
}
