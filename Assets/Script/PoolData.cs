// using System.Collections.Generic;
// using UnityEngine;

// public class PoolData
// {
//     private Dictionary<int, Data> poolTable = new Dictionary<int, Data>();
//     public class Data
//     {
//         // 풀 ID
//         public int id;
//         // 풀 이름
//         public string poolName;
//         // 프리팹 가져올 어드레서블 주소 
//         public string addressableKey;
//         // 풀 초기크기            
//         public int poolCount;
//         // 풀 최대 크기
//         public int maxPoolCount;
//         // 적용할 스테이지 
//         public int stageId;
//     }

//     public PoolData()
//     {
//         // 풀 데이터 추가
//         poolTable.Add(1, new Data()
//         {
//             id = 1,
//             poolName = "BulletPool",
//             addressableKey = "Bullet",
//             poolCount = 20,
//             stageId = 1,
//         });
//         poolTable.Add(2, new Data()
//         {
//             id = 2,
//             poolName = "MissilePool",
//             addressableKey = "Missile",
//             poolCount = 10,
//             stageId = 1,
//         });
//         poolTable.Add(3, new Data()
//         {
//             id = 3,
//             poolName = "LaserPool",
//             addressableKey = "Laser",
//             poolCount = 15,
//             stageId = 2,
//         });
//     }

//     public Data GetData(int id)
//     {
//         if (!poolTable.ContainsKey(id))
//         {
// #if DEBUG_MODE
//             throw new System.Exception($"poolTable ID: {id} not found.");
// #endif
//             return null;
//         }

//         return poolTable[id];
//     }
// }
