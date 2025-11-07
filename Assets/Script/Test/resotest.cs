using UnityEngine;
using Cysharp.Threading.Tasks;  
public class resotest : MonoBehaviour
{    
    void Start()
    {
        DataTableManager.LoadAllAsync().Forget();         
    }
}
