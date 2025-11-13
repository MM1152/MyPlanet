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
        //Debug.Log("Walk State Enter");
#endif
        target = enemy.GetTarget();
        enemy.speed = enemy.CurrentSpeed;
    }
    


    public void Execute()
    {
        if(target == null)
        {           
            return;
        }

        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, target.transform.position, enemy.CurrentSpeed * Time.deltaTime);
    }

    public void Exit()
    {
       #if DEBUG_MODE
        //Debug.Log("Walk State Exit");  
         #endif     
    }
}
