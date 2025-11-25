using UnityEngine;

public class SplitbornAbility : BaseAbility
{
    public override AbilityType abilityType => AbilityType.OnDamage;
    public int spawnCount = 1;

    public override bool isActive
    {
        get;
        set;
    }

    public override int OnDamage(int damage)
    {
        if (!isActive) return damage;

        Split();

        return damage;
    }
    
    private void Split()
    {
        isActive = false;
        var waveManager = enemy.WaveManager;
        var enemySpawnManager = waveManager.EnemySpawnManager;

        var spawnEnemys = enemySpawnManager.SpawnEnemy(enemy.enemyData.ID, spawnCount);
        foreach (var spawnEnemy in spawnEnemys)
        {
            var offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
            spawnEnemy.transform.position = enemy.transform.position + offset;
            waveManager.totalEnemyCount++;
            waveManager.waveClearCount++;
            spawnEnemy.ability.isActive = false;
        }
    }

}
