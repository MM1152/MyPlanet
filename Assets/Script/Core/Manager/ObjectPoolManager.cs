using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using System;
using System.Threading.Tasks;

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

    // 풀 사이즈를 조절하는데 현재 풀사이즈 유지를 얼마나할지 몰라서 일단 넣어둠
    private void OnDestoryObject(GameObject obj)
    {
        Destroy(obj);
    }

    // 오브젝트 스폰 (풀에서 오브젝트 가져오기)
    public T SpawnObject<T>(PoolsId id)
    {
        if (!ObjPools.ContainsKey(id))
        {
#if DEBUG_MODE
            Debug.LogError($"Pool ID {id}가 존재하지 않습니다.");
#endif
            return default;
        }

        GameObject obj = ObjPools[id].Get();
        if (obj != null)
        {
            T component = obj.GetComponent<T>();
#if DEBUG_MODE
            // Debug.Log($"{component} 생성");
#endif
            return component;
        }
        return default;
    }
    
    public void Despawn(PoolsId id, GameObject obj)
    {
        if (ObjPools.ContainsKey(id))
        {
#if DEBUG_MODE
            // Debug.Log($"디스폰 호출");
#endif
            ObjPools[id].Release(obj);
        }
    }

    private void ClearPool(PoolsId id)
    {
        if (!ObjPools.ContainsKey(id))
            return;

#if DEBUG_MODE
        Debug.Log($"Pool {id} 클리어 호출");
#endif
        
        // 풀의 모든 오브젝트를 제거
        ObjPools[id].Clear();
        ObjPools.Remove(id);
    }

    private void ClearAllPools()
    {
#if DEBUG_MODE
        Debug.Log($"모든 풀 클리어 시작 - 총 {ObjPools.Count}개 풀");
#endif
        // Dictionary를 순회하면서 모든 풀 제거
        foreach (var pool in ObjPools)
        {
            pool.Value.Dispose();
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
#if DEBUG_MODE
        Debug.Log("모든 풀 클리어 완료");
#endif
    }

    public void Release()
    {
        ClearAllPools();
    }

    private void OnDestroy()
    {
        // 씬 전환 시 자동으로 풀 정리
        Release();
    }
}








