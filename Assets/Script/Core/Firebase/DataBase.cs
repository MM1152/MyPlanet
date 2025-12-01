using Cysharp.Threading.Tasks;
using Firebase.Database;
using System;
using System.Collections.Generic;
using UnityEngine;
public class DataBase 
{
    private FirebaseDatabase database;
    private DatabaseReference root;

    private Dictionary<DatabaseReference, List<EventHandler<ValueChangedEventArgs>>> listners
        = new Dictionary<DatabaseReference, List<EventHandler<ValueChangedEventArgs>>>();

    public void Init()
    {
        database = FirebaseDatabase.DefaultInstance;
        database.SetPersistenceEnabled(false);
        root = database.RootReference;

    }

    public void AddListner(string path , EventHandler<ValueChangedEventArgs> callback)
    {
        DatabaseReference newReference = root.Child(path);
        if(!listners.ContainsKey(newReference))
        {
            listners.Add(newReference  , new List<EventHandler<ValueChangedEventArgs>>() { callback });
        }
        else
        {
            listners[newReference].Add(callback);
        }

        newReference.ValueChanged += callback;
    }

    public void RemoveListner(string path, EventHandler<ValueChangedEventArgs> callback)
    {
        DatabaseReference newReference = root.Child(path);
        if (listners.ContainsKey(newReference))
        {
            if (listners[newReference].Contains(callback))
            {
                newReference.ValueChanged -= callback;
                listners[newReference].Remove(callback);
            }
        }

    }

    public void Release()
    {
        foreach(var key in listners.Keys)
        {
            for(int i = 0; i < listners[key].Count; i++)
            {
                key.ValueChanged -= listners[key][i];
            }
        }

        listners.Clear();
    }

    public async UniTask<(int version , bool success)> GetVersion()
    {
        DatabaseReference versionRef = root.Child("Version");

        if (versionRef == null) return (0, false);

        try
        {
            DataSnapshot snapshot = await versionRef.GetValueAsync().AsUniTask();
            if(!snapshot.Exists)
            {
                throw new System.Exception($"Empty Value Version");
            }
            
            return (int.Parse(snapshot.Value.ToString()) , true);
        }
        catch(System.Exception ex)
        {
            Debug.LogException(ex);
            return (0, false);
        }
    }

    /// <summary>
    /// 파이어베이스에서 데이터 가져오기 및 T로 변환
    /// </summary>
    public async UniTask<(T data, bool success)> GetData<T>(string path) where T : JsonSerialized
    {
        T data = default;

        DatabaseReference newReference = root.Child(path);
        if (newReference == null)
            return (data , false);

        try
        {
            DataSnapshot snapshot = await newReference.GetValueAsync().AsUniTask();
            if (!snapshot.Exists)
                throw new System.Exception($"Empty Value in Firebase database : {path}");

            data = JsonSerialized.FromJson<T>(snapshot.GetRawJsonValue());
            data.PushId = snapshot.Key;
            return (data, true);
        }       
        catch (System.Exception ex)
        {
#if DEBUG_MODE
            Debug.LogError($"Firebase Database GetData Fail : {ex}");
#endif
            return (data, false);
        }
    }

    public async UniTask<(object data, bool success)> GetDataToValue(string path)
    {
        object data = default;

        DatabaseReference newReference = root.Child(path);
        if (newReference == null)
            return (data, false);

        try
        {
            DataSnapshot snapshot = await newReference.GetValueAsync().AsUniTask();
            if (!snapshot.Exists)
                throw new System.Exception($"Empty Value in Firebase database : {path}");

            data = snapshot.Value;
            
            return (data, true);
        }
        catch (System.Exception ex)
        {
#if DEBUG_MODE
            Debug.LogError($"Firebase Database GetData Fail : {ex}");
#endif
            return (data, false);
        }
    }

    /// <summary>
    /// 해당 path의 모든 데이터를 T형식 리스트로 변환
    /// </summary>
    public async UniTask<(List<T> data, bool success)> GetDatas<T>(string path) where T : JsonSerialized
    {
        List<T> data = new List<T>();

        DatabaseReference newReference = root.Child(path);
        if (newReference == null)
            return (data, false);

        try
        {
            DataSnapshot snapshot = await newReference.GetValueAsync().AsUniTask();
            if (!snapshot.Exists)
                throw new System.Exception($"Empty Value in Firebase database : {path}");

            foreach(var snapshotChild in snapshot.Children) 
            {
                var inputData = JsonSerialized.FromJson<T>(snapshotChild.GetRawJsonValue());
                inputData.PushId = snapshotChild.Key;
                data.Add(inputData);
            }
            return (data, true);
        }
        catch (System.Exception ex)
        {
#if DEBUG_MODE
            Debug.LogError($"Firebase Database GetDatas Fail : {ex}");
#endif
            return (data, false);
        }
    }

    /// <summary>
    /// 새로운 데이터로 덮어쓰기
    /// </summary>
    public async UniTask<bool> OverwriteJsonData<T>(string path, T json) where T : JsonSerialized
    {
        DatabaseReference newReference = root.Child(path);

        try
        {
            Debug.Log(json.ToJson());
            await newReference.SetRawJsonValueAsync(json.ToJson()).AsUniTask(); 
            return true;
        }
        catch (System.Exception ex)
        {
#if DEBUG_MODE
            Debug.LogError($"Firebase Database Overwrite Fail : {ex}");
#endif
            return false;
        }
    }

    /// <summary>
    /// 새로운 데이터 푸쉬 한뒤 밑에 덮어 쓸 때 사용
    /// </summary>
    public async UniTask<bool> PushWirteJsonData<T>(string path, T json) where T : JsonSerialized
    {
        DatabaseReference newReference = root.Child(path);

        try
        {
            var childRef = newReference.Push();
            json.PushId = childRef.Key;
            await childRef.SetRawJsonValueAsync(json.ToJson()).AsUniTask();
            return true;
        }
        catch (System.Exception ex)
        {
#if DEBUG_MODE
            Debug.LogError($"Firebase Database PushWirte Fail : {ex}");
#endif
            return false;
        }
    }
}