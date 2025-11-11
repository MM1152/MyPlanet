using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;
using Cysharp.Threading.Tasks;
using UnityEditor;
using Unity.VisualScripting;
using Cysharp.Threading.Tasks.Triggers;


public class ObjectPoolManager : MonoBehaviour
{

    // 관리할 오브젝트 id  , 오브젝트 풀
    private static Dictionary<int, ObjectPool<GameObject>> ObjPools = new Dictionary<int, ObjectPool<GameObject>>();


    private static ObjectPoolManager instance;
    public static ObjectPoolManager Instance => instance;

    private static Transform poolsRoot;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            var poolRoot = new GameObject("PoolsRoot");
            poolRoot.transform.SetParent(this.transform, false);
            poolsRoot = poolRoot.transform;
            //스테이지가 바뀌어도 오브젝트 풀 매니저가 파괴되지 않도록 설정            
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 오브젝트 풀 생성
    private static void CreatePool(int id, GameObject prefab)
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
    private static GameObject CreateObject(GameObject prefab)
    {
        var obj = Instantiate(prefab, poolsRoot);
        obj.SetActive(false);
        return obj;
    }
    // 오브젝트 활성화    
    private static void OnGetObject(GameObject obj)
    {
        obj.SetActive(true);
    }
    // 오브젝트 비활성화
    private static void OnReleaseObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    //풀 사이즈를 조절하는데 현재 풀사이즈 유지를 얼마나할지 몰라서 일단 넣어둠
    private static void OnDestoryObject(GameObject obj)
    {
        Destroy(obj);
    }

    // 오브젝트 스폰 (풀에서 오브젝트 가져오기)
    public static T SpawnObject<T>(int id, GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!ObjPools.ContainsKey(id))
        {
            CreatePool(id, prefab);
        }

        GameObject obj = ObjPools[id].Get();
        if (obj != null)
        {
            obj.transform.position = position;
            obj.transform.rotation = rotation;


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
    private static void Despawn(int id, GameObject obj)
    {
        if (ObjPools.ContainsKey(id))
        {
#if DEBUG_MODE
            Debug.Log($"디스폰 호출");
#endif
            ObjPools[id].Release(obj);
        }
    }

    private static void ClearPool(int id)
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

    private static void ClearAllPools()
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








