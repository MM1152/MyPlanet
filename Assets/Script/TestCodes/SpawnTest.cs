using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnTest : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    private Queue<GameObject> spawnedEnemy = new Queue<GameObject>();   
    private void Update()
    {
        // Vector3 mousePos = Mouse.current.position.ReadValue();
        //  Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {            
         spawnedEnemy.Enqueue(ObjectPoolManager.SpawnObject(1, enemyPrefab, new Vector3(0f, 10f, 0f), Quaternion.identity));
        }

        if( Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (spawnedEnemy.Count > 0)
            {
                var enemyToDie = spawnedEnemy.Dequeue();
                enemyToDie.GetComponent<Enemy>().stateMachine.ChangeState(enemyToDie.GetComponent<Enemy>().stateMachine.dieState);  
            }
        }     
    }
}
