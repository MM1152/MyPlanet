using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;


public class ObjectPoolManager
{
    // 관리할 오브젝트 id  , 오브젝트 풀
    private Dictionary<int, ObjectPool<GameObject>> ObjPools = new Dictionary<int, ObjectPool<GameObject>>();
    private static GameObject root;

    private static ObjectPoolManager instance;
    public static ObjectPoolManager Instance 
    {
        get
        {
            if( instance == null )
            {
                instance = new ObjectPoolManager();
                root = new GameObject("ObjectPools");
                Object.DontDestroyOnLoad(root);
                poolsRoot = root.transform;
            }
            return instance;
        }
    }

    private static Transform poolsRoot;

    // 오브젝트 풀 생성
    private void CreatePool(int id, GameObject prefab)
    {
        var newPool = new ObjectPool<GameObject>(
          createFunc: () => CreateObject(prefab),
          actionOnGet: OnGetObject,
          actionOnRelease: OnReleaseObject,
          actionOnDestroy: OnDestoryObject,
          collectionCheck: false,
          defaultCapacity: 10,
          maxSize: 100
        );

        ObjPools.Add(id, newPool);
    }

    // 오브젝트 생성
    private GameObject CreateObject(GameObject prefab)
    {
        var obj = GameObject.Instantiate(prefab, poolsRoot);
        obj.SetActive(false);
        return obj;
    }
    // 오브젝트 활성화    
    private void OnGetObject(GameObject obj)
    {
        obj.SetActive(true);
    }
    // 오브젝트 비활성화
    private void OnReleaseObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    //풀 사이즈를 조절하는데 현재 풀사이즈 유지를 얼마나할지 몰라서 일단 넣어둠
    private void OnDestoryObject(GameObject obj)
    {
        GameObject.Destroy(obj);
    }

    // 오브젝트 스폰 (풀에서 오브젝트 가져오기)
    public T SpawnObject<T>(int id, GameObject prefab)
    {
        if (!ObjPools.ContainsKey(id))
        {
            CreatePool(id, prefab);
        }

        GameObject obj = ObjPools[id].Get();
        if (obj != null)
        {
            T component = obj.GetComponent<T>();
#if DEBUG_MODE
            if (component == null)
            {
                Debug.LogWarning($"Component {typeof(T).Name} not found on {prefab.name}");
            }
#endif

            return component;
        }
        return default;
    }
    public void Despawn(int id, GameObject obj)
    {
        if (ObjPools.ContainsKey(id))
        {
#if DEBUG_MODE
            Debug.Log($"디스폰 호출");
#endif
            ObjPools[id].Release(obj);
        }
    }

    private void ClearPool(int id)
    {
        if (ObjPools.ContainsKey(id))
        {
#if DEBUG_MODE
            Debug.Log($"클리어 호출");
#endif
            ObjPools[id].Clear();
            ObjPools.Remove(id);
        }
    }

    private void ClearAllPools()
    {
        foreach (var pool in ObjPools.Values)
        {

            pool.Clear();
        }
#if DEBUG_MODE
        Debug.Log($"클리어 올 호출");
#endif
        ObjPools.Clear();
    }
}








