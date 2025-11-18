using UnityEngine;
using System.Collections.Generic;   

public interface IAttack
{
    // 콜라이더를 검사할건지 
    public bool isAttackColliderOn { get; }
    //각 공격별로 재정의할 공격 메서드
    public void Attack(Enemy enemy);  
}
