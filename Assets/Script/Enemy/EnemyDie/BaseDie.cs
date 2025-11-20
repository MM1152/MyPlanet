using UnityEngine;

public class BaseDie : MonoBehaviour
{
    protected Enemy enemy;
    protected TypeEffectiveness typeEffectiveness;

    public virtual void Init(Enemy enemy)
    {
        this.enemy = enemy;
        this.typeEffectiveness = enemy.typeEffectiveness;
    }

    public virtual void Die(Enemy enemy)
    {        
        enemy.WaveManager.totalEnemyCount--;    
        enemy.WaveManager.waveClearCount--;        
        enemy.IsDead = true;
        Managers.ObjectPoolManager.Despawn(PoolsId.Enemy, enemy.gameObject);
    }
}
