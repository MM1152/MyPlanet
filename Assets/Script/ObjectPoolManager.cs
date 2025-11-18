using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using System;

public class ObjectPoolManager : MonoBehaviour
{
    // 관리할 오브젝트 id  , 오브젝트 풀
    private Dictionary<PoolsId, ObjectPool<GameObject>> ObjPools = new Dictionary<PoolsId, ObjectPool<GameObject>>();
    
    public async UniTask Init()
    {
        var assets = await Addressables.LoadAssetsAsync<GameObject>(AddressableLabelIds.PoolsIds).ToUniTask();

        foreach(var asset in assets)
        {
            var id = AddressableNames.GetPoolsId(asset.name);
            if (id != PoolsId.None)
            {
                CreatePool(id, asset);
            }
            else
            {
                Debug.Log($"Addrassable Asset Load Fail To {asset.name}");
            }
        }
    }

    // 오브젝트 풀 생성
    private void CreatePool(PoolsId id, GameObject prefab)
    {
        var newPool = new ObjectPool<GameObject>(
          createFunc: () => CreateObject(prefab),
          actionOnGet: OnGetObject,
          actionOnRelease: OnReleaseObject,
          actionOnDestroy: OnDestoryObject,
          collectionCheck: false,
          defaultCapacity: 10,
          maxSize: 1000
        );

        ObjPools.Add(id, newPool);
    }

    // 오브젝트 생성
    private GameObject CreateObject(GameObject prefab)
    {
        var obj = Instantiate(prefab, transform);
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
        Destroy(obj);
    }

    // 오브젝트 스폰 (풀에서 오브젝트 가져오기)
    public T SpawnObject<T>(PoolsId id)
    {

        GameObject obj = ObjPools[id].Get();
        if (obj != null)
        {
            T component = obj.GetComponent<T>();
            //  Debug.Log(component.ToString() + " 생성");
            return component;
        }
        return default;
    }
    public void Despawn(PoolsId id, GameObject obj)
    {
        if (ObjPools.ContainsKey(id))
        {
#if DEBUG_MODE
            //Debug.Log($"디스폰 호출");
#endif
            ObjPools[id].Release(obj);
        }
    }

    private void ClearPool(PoolsId id)
    {
        if (ObjPools.ContainsKey(id))
        {
#if DEBUG_MODE
            //Debug.Log($"클리어 호출");
#endif
            ObjPools[id].Clear();
            ObjPools.Remove(id);
        }
    }

    private void ClearAllPools()
    {
        foreach (var pool in ObjPools.Values)
        {
            for(int i = 0; i < pool.CountAll; i++)
            {
                var obj = pool.Get();
                Destroy(obj);
            }
            pool.Clear();
        }
#if DEBUG_MODE
        Debug.Log($"클리어 올 호출");
#endif
        ObjPools.Clear();
    }

    public void Release()
    {
        ClearAllPools();
    }
}








