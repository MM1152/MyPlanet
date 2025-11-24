using UnityEngine;

public class BaseDie
{
    protected Enemy enemy;
    protected TypeEffectiveness typeEffectiveness;

    public virtual bool active { get; set; } = true;

    public virtual void Init(Enemy enemy)
    {
        this.enemy = enemy;
        this.typeEffectiveness = enemy.typeEffectiveness;
    }

    public virtual void Die(Enemy enemy)
    {        
        enemy.WaveManager.totalEnemyCount--;    
        enemy.WaveManager.waveClearCount--;        
        Managers.ObjectPoolManager.Despawn(PoolsId.Enemy, enemy.gameObject);
    }
}
