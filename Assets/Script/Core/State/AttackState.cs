using NUnit.Framework;
using UnityEngine;

public class AttackState : IState
{
    private Enemy enemy;

    public AttackState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
#if DEBUG_MODE
       // Debug.Log("Attack State Enter");  
#endif

    }

    public void Execute()
    {
        var target = enemy.GetTarget();
        target.GetComponent<IDamageAble>().OnDamage(enemy.enemyData.ATK);
    }

    public void Exit()
    {
#if DEBUG_MODE
       // Debug.Log("Attack State Exit");  
#endif
    }
}
