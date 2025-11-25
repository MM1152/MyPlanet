using UnityEngine;

public class SpawnDie : BaseDie
{
    private int spawnEnemyID = 3005;    
    private bool bonusApplied = false;  
    private int baseCount = 3;
    public int spawnCount { get; set; } = 3;
    public override void Die(Enemy enemy)
    {
        if (enemy.currentHP <= 0)
        {
            var waveManager = enemy.WaveManager;
            var enemySpawnManager = waveManager.EnemySpawnManager;

            var spawnEnemys = enemySpawnManager.SpawnEnemy(spawnEnemyID, spawnCount);
            foreach (var spawnEnemy in spawnEnemys)
            {
                var offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
                spawnEnemy.transform.position = enemy.transform.position + offset;
                waveManager.totalEnemyCount++;
                waveManager.waveClearCount++;
            }
        }

        base.Die(enemy);
    }

    public override void SetBonusCount(int count)
    {
        if (bonusApplied) return;
        spawnCount = baseCount + count;
        bonusApplied = true;
    }
    public override void ResetCount()
    {
        spawnCount = baseCount;
        bonusApplied = false;
    }
}
