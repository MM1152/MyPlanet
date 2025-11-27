using Cysharp.Threading.Tasks;
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
        public int star;

        public bool UseAble => level != 0;
        // -1 : 닫혀있음 , 0 : 열려있음
        public List<int> openSlot;
        public Data(int id)
        {
            this.id = id;
            level = 0;
            count = 0;
            star = 0;
            openSlot = new List<int>();

            var cnt = DataTableManager.PlanetTable.Get(id).InitOpenSlotCount;
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

        public int NeedPeiceCount => star switch
        {
            0 => DataTableManager.OptionTable.GetValueDataToInt(5050),
            1 => DataTableManager.OptionTable.GetValueDataToInt(5051),
            2 => DataTableManager.OptionTable.GetValueDataToInt(5052),
            3 => DataTableManager.OptionTable.GetValueDataToInt(5053),
            4 => DataTableManager.OptionTable.GetValueDataToInt(5054),
            _ => 0
        };
    
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
        await UniTask.WaitUntil(() => FirebaseManager.Instance.UserId != string.Empty);

        var path = DataBasePaths.PlanetPath + FirebaseManager.Instance.UserId + "/";
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
            Debug.Log("Load Planet Data sussess");
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

    private async UniTask SaveAsync(int planetId)
    {
        var path = DataBasePaths.PlanetPath + FirebaseManager.Instance.UserId + $"/{planetId}";
        var saveData = new Data(planetId);
        if(planetId == 1001)
        {
            saveData.level = 1;
        }
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

    public async UniTask LevelUpPlanetAsync(int planetId)
    {
        var path = DataBasePaths.PlanetPath + FirebaseManager.Instance.UserId + $"/{planetId}";
        planetsTable[planetId].level++;

        await FirebaseManager.Instance.Database.OverwriteJsonData(path, planetsTable[planetId]);
    }

    public async UniTask UpgradeStarAsync(int planetId , int usePieceCount)
    {
        var path = string.Format(DataBasePaths.PlanetDataPathFormating, planetId);
        planetsTable[planetId].star++;
        planetsTable[planetId].count -= usePieceCount;
        await FirebaseManager.Instance.Database.OverwriteJsonData(path, planetsTable[planetId]);
    }

    public async UniTask UnLockSlotAsync(int planetId, int unlockIdx)
    {
        var path = string.Format(DataBasePaths.PlanetDataPathFormating, planetId);
        planetsTable[planetId].openSlot[unlockIdx] = 0;
        await FirebaseManager.Instance.Database.OverwriteJsonData(path, planetsTable[planetId]);
    }
    public void Release()
    {
        planetsTable.Clear();
        init = false;
    }
}
