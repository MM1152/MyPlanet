using UnityEngine;

public class OneTimeMeleeAttacker : IAttack
{
    public bool isAttackColliderOn => true;
    public void Attack(Enemy enemy)
    {
        var target = enemy.GetTarget();
        var find = target.GetComponent<IDamageAble>();

        float percent = enemy.typeEffectiveness.GetDamagePercent(find.ElementType);  
        find.OnDamage(Mathf.Clamp((int)((enemy.atk-find.Defense)* percent), 1, int.MaxValue));             
        enemy.OnDead();
    }
}
