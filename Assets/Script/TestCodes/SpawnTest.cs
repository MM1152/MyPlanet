using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnTest : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    //private void Update()
    //{
    //    //Vector3 mousePos = Mouse.current.position.ReadValue();
    //    // Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        
    //    //if (Keyboard.current.tabKey.wasPressedThisFrame)
    //    //{
    //    //    ObjectPoolManager.SpawnObject<Enemy>(enemyPrefab.GetInstanceID(), enemyPrefab, new Vector3(worldPos.x, worldPos.y, 0f), Quaternion.identity);
    //    //}
    //}
}
