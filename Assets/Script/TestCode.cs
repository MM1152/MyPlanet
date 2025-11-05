using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour 
{
    private async void Start()
    {
        await FirebaseManager.Instance.WaitForInitalizedAsync();
    }
}
