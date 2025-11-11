using UnityEngine;

public class WalkState : IState
{
    private Enemy enemy;
    private GameObject target;
    public WalkState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
#if DEBUG_MODE
        Debug.Log("Walk State Enter");
#endif
        target = enemy.GetTarget();
    }
    


    public void Execute()
    {
        if(target == null)
        {           
            return;
        }
        enemy.speed = enemy.enemyData.Speed;
      
        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, target.transform.position, enemy.speed * Time.deltaTime);
    }

    public void Exit()
    {
       #if DEBUG_MODE
        Debug.Log("Walk State Exit");  
         #endif     
    }
}
