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
        if (enemy.isKilledByPlayer)
        {
            var exp = Managers.ObjectPoolManager.SpawnObject<Exp>(PoolsId.Exp);
            exp.transform.position = enemy.transform.position;
            exp.exp = enemy.enemyData.EXP;
        }       
        if(enemy.WaveManager != null)
        {
            enemy.WaveManager.totalEnemyCount--;
            enemy.WaveManager.waveClearCount--;
        }
    
        Managers.ObjectPoolManager.Despawn(PoolsId.Enemy, enemy.gameObject);
    }

    public virtual void SetBonusCount(int count) {}   
    public virtual void ResetCount() {} 
}
