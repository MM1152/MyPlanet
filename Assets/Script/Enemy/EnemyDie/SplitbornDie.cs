using UnityEngine;

public class SplitbornDie : BaseDie
{
    public int spawnCount { get; set; } = 2;

    public override void Die(Enemy enemy)
    {
        if (enemy.currentHP <= 0 && active)
        {
            var waveManager = enemy.WaveManager;
            var enemySpawnManager = waveManager.EnemySpawnManager;

            var spawnEnemys = enemySpawnManager.SpawnEnemy(enemy.enemyData.ID, spawnCount);
            foreach (var spawnEnemy in spawnEnemys)
            {
                var offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
                spawnEnemy.transform.position = enemy.transform.position + offset;
                spawnEnemy.die.active = false;                
                waveManager.totalEnemyCount++;
                waveManager.waveClearCount++;
            }
            active = false;            
        }
        base.Die(enemy);
    }
}
