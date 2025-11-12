using UnityEngine;
using UnityEngine.InputSystem;

public class WaveTestet : MonoBehaviour
{
    [SerializeField] private EnemySpawnManager enemySpawnManager;

    private void Update()
    {
        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            var enemy = enemySpawnManager.SpawnEnemy(1);
            if(enemy != null)
            {

            }
        }
    }
}
