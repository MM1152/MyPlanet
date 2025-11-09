using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;


public class ObjectPoolManager : MonoBehaviour
{

    // 키: 풀 id , 값: 오브젝트 풀
    private Dictionary<int, ObjectPool<GameObject>> poolDictionary = new Dictionary<int, ObjectPool<GameObject>>();

    // 풀 초기화 메서드 / 풀 생성/  AddressableManager 통해 프리팹 로드 / 키 검사 // 
    public async UniTask InitPool(PoolData.Data poolData)
    {
        if (poolDictionary.ContainsKey(poolData.id))
        {
#if DEBUG_MODE
            throw new Exception($"이러한 ID: {poolData.id} 풀이 이미 있어요 .");
#endif
            return;
        }
        // 프리펩 어드레서블 로드함 
        GameObject prefab = await Addressables.LoadAssetAsync<GameObject>(poolData.addressableKey).ToUniTask();
        // 로드한 프리팹으로 오브젝트 풀 생성 // 각 콜백 함수 작성 연결    
        ObjectPool<GameObject> newPool = new ObjectPool<GameObject>(
           createFunc: () => Instantiate(prefab),
           actionOnGet: (obj) => obj.SetActive(true),
           actionOnRelease: (obj) => obj.SetActive(false),
           actionOnDestroy: (obj) => Destroy(obj),
           collectionCheck: false,
           defaultCapacity: poolData.poolCount,
           maxSize: poolData.maxPoolCount
        );
        // 생성한 풀을 딕셔너리에 추가
        poolDictionary.Add(poolData.id, newPool);
    }
    // 객체 가져오기 메서드 / 객체 반환 메서드 //
    public GameObject GetObject(int poolId)
    {
        if (poolDictionary.TryGetValue(poolId, out var pool))
        {
            return pool.Get();
        }
        return null;    
    }
     // 객체 반환 메서드 // id  검사 벨류접근 
    public void ReleaseObject(int id, GameObject obj)
    {
        if (poolDictionary.TryGetValue(id, out var pool))
        {
            pool.Release(obj);
        }
    }
    // 풀 존재 여부 확인 메서드 //
    public bool HasPool(int id)
    {
        return poolDictionary.ContainsKey(id);
    }

     //id  검사 벨류접근 비우고 id 삭제
    public void ClearPool(int id)
    {
       if(poolDictionary.TryGetValue(id, out var pool))
        {
            pool.Clear();
            poolDictionary.Remove(id);
        }
    }
}



 




