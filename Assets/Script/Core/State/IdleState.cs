using UnityEngine;

public class IdleState : IState
{
    private Enemy enemy;
   
    public IdleState(Enemy enemy)
    {
        this.enemy = enemy;
    }
    // 진입 애니메이션 
     public void Enter()
    {
       #if DEBUG_MODE
        //Debug.Log("Idle State Enter");  
         #endif
    }

    public void Execute()
    {
        
    }
    // 애니메이션 종료 
    public void Exit()
    {    
        #if DEBUG_MODE
        //Debug.Log("Idle State Exit");  
         #endif
           
    }

}
