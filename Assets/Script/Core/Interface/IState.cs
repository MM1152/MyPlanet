using UnityEngine;

public interface IState
{
    // 상태 진입     
    public void Enter();
    //실행 
    public void Execute();
    //종료
    public void Exit();


}
