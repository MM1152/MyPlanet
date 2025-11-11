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
        Debug.Log("Idle State Enter");  
         #endif
    }

    public void Execute()
    {
        // 이동 속도 0으로 설정
        enemy.speed = 0f;
    }
    // 애니메이션 종료 
    public void Exit()
    {    
        #if DEBUG_MODE
        Debug.Log("Idle State Exit");  
         #endif
           
    }

}
