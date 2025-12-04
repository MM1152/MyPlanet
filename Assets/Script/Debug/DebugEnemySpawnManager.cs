using UnityEngine;

public class DebugEnemySpawnManager : EnemySpawnManager
{
    protected override void Awake()
    {
        base.Awake();
        Variable.IsDebugMode = true;
    }
}
