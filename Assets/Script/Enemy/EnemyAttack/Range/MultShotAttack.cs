using System.Collections.Generic;
using UnityEngine;

public class MultShotAttack : IShotStrategy
{
    private List<IShotStrategy> shotStrategies;
    
    private IShotStrategy currentStrategy;

    public MultShotAttack(params IShotStrategy[] shots)
    {
        shotStrategies = new List<IShotStrategy>(shots);
        Debug.Log($"[MultShotAttack] 총 {shotStrategies.Count}개의 샷 전략이 등록되었습니다.");
    }

    public void Shot(Enemy enemy, GameObject target)
    {
        if(shotStrategies == null || shotStrategies.Count != 2) return;

         if(enemy.move is BaseBossMonsterMove bossMove)
         {
            if(bossMove.currentStrategy is OneTwoMoveAttack oneTwoMove)
            {
                 currentStrategy = oneTwoMove.useVerticalPattern ==true ? shotStrategies[0] : shotStrategies[1];
                 Debug.Log($"[MultShotAttack] 현재 패턴에 따라 {(oneTwoMove.useVerticalPattern ==true ? "두번째" : "첫번째")} 샷 전략이 선택되었습니다.");
                 currentStrategy.Shot(enemy, target);
            }  
         }
    }
}
