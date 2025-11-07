using UnityEngine;

public class TestCode1 : MonoBehaviour
{
    public void Start()
    {
        SafeAreaManager.ReplacePositionGameObject(this.gameObject);   
        SafeAreaManager.ReplaceScaleGameObject(this.gameObject);   
    }
}