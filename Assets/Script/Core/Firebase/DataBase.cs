using Cysharp.Threading.Tasks;
using Firebase.Database;
using UnityEngine;
public class DataBase 
{
    private FirebaseDatabase database;
    private DatabaseReference root;

    public void Init()
    {
        database = FirebaseDatabase.DefaultInstance;
        root = database.RootReference;
    }

    /// <summary>
    /// Get data from FirebaseDatabase
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path">  </param>
    /// <returns></returns>
    public async T GetData<T>(string path) 
    {
        T data = default;

        DatabaseReference newReference = root;

        string[] splits = path.Split('/');
        for(int i = 0; i < splits.Length; i++)
        {
            newReference = newReference.Child(splits[i]);
            if(newReference == null)
            {
#if DEBUG_MODE
                Debug.LogError($"Firebase Database GetData Fail : {path}");
#endif
                break;
            }
        }

        try
        {
            DataSnapshot snapshot = await newReference.GetValueAsync().AsUniTask();
            return snapshot as T;
        }
        catch (System.Exception ex)
        {
#if DEBUG_MODE
            Debug.LogError($"Firebase Database GetData Fail : {ex}");
#endif
        }



    }
}