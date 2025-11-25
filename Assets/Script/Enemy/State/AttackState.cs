using UnityEngine;

public class AttackState : IState
{
    private Enemy enemy;

    private GameObject target;

    public AttackState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
         this.target = enemy.GetTarget();
    }

    public void Execute()
    {
        if(target == null)
        {
            enemy.stateMachine.ChangeState(enemy.stateMachine.idleState);
            return;
        }

        if (enemy.attackRange <= 0 || Vector2.Distance(enemy.transform.position, target.transform.position) <= enemy.attackRange)
        {
            Debug.Log($"적 타입{enemy.enemyType} 공격 실행");   
            enemy.attack.Attack(enemy);
            enemy.ReturnMoveAction?.Invoke(); 
            return;
        }

        enemy.stateMachine.ChangeState(enemy.stateMachine.walkState);
    }

    public void Exit()
    {
        if (enemy.attack is ShotAttack shotAttack &&
        shotAttack.GetShotStrategy(enemy.ElementType) is LaserShot laserShot)
        {
            laserShot.LaserReset();
        }
    }
}
