using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using System;
using System.Collections.Generic;
using UnityEngine;
//Firebase 저장용 데이터
public class PlanetData
{
    private bool init;
    private Dictionary<int, Data> planetsTable = new Dictionary<int, Data>();
    
    [Serializable]
    public class Data : JsonSerialized
    {
        public int id;
        public int level;
        public int count;
        // -1 : 닫혀있음 , 0 : 열려있음
        public List<int> openSlot;
        public Data(int id)
        {
            this.id = id;
            level = 0;
            count = 0;
            openSlot = new List<int>();
            var grade = DataTableManager.PlanetTable.Get(id).grade;
            var cnt = 0;

            if (grade == "S") cnt = 7;
            else if (grade == "A") cnt = 6;
            else if (grade == "B") cnt = 5;
            else if (grade == "C") cnt = 4;

            for(int i = 0; i < 12; i++)
            {
                openSlot.Add(-1);
                if(cnt > 0)
                {
                    openSlot[i] = 0;
                    cnt--;
                }
            }
        }
    }

    public Data GetDeepCopy(int id)
    {
        var copyData = new Data(id);
        return copyData;
    }

    public Data GetOrigin(int id)
    {
        return planetsTable[id];
    }

    public async UniTask LoadAllDataAsync()
    {
        await UniTask.WaitUntil(() => FirebaseManager.Instance.UserData != null);

        var path = DataBasePaths.PlentPath + FirebaseManager.Instance.UserId + "/";
        var success = await FirebaseManager.Instance.Database.GetDatas<Data>(path);

        if(success.success)
        {
            // 데이터가 있다면
            if(FirebaseManager.Instance.ChangeVersion)
            {
                Debug.Log("Update Planet Data Version : ");
                for (int i = 0; i < success.data.Count; i++)
                {
                    var updatePath = path + success.data[i].id + '/';
                    Debug.Log("Update Planet Data Version : " + success.data[i].id);
                    await FirebaseManager.Instance.Database.OverwriteJsonData(updatePath, success.data[i]);
                }
            }

            for(int i = 0; i < success.data.Count; i++)
            {
                if(!planetsTable.ContainsKey(success.data[i].id))
                {
                    planetsTable.Add(success.data[i].id, success.data[i]);
                }
                else
                {
                    planetsTable[success.data[i].id] = success.data[i];
                }
            }
        }
        else
        {
            // 데이터가 없다면
            var planets = DataTableManager.PlanetTable.GetAllData();
            for (int i = 0; i < planets.Count; i++)
            {
                await SaveAsync(planets[i].ID);
            }
        }

        init = true;
    }

    public async UniTask SaveAsync(int planetId)
    {
        var path = DataBasePaths.PlentPath + FirebaseManager.Instance.UserId + $"/{planetId}";
        var saveData = new Data(planetId);
        var success = await FirebaseManager.Instance.Database.OverwriteJsonData(path, saveData);

        if(success)
        {
            // 저장 성공
            Debug.Log ("Planet Data Save Success");
            if(!planetsTable.ContainsKey(planetId))
            {
                planetsTable.Add(planetId, saveData);
            }
            else
            {
                planetsTable[planetId] = saveData;
            }
        }
        else
        {
            // 저장 실패
            Debug.Log("Planet Data Save Fail");
        }
    }

    public async UniTask WaitForInitalizeAsync()
    {
        await UniTask.WaitUntil(() => init);
    }

    public void Release()
    {
        planetsTable.Clear();
        init = false;
    }
}
