using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerData
{
    public bool Init { get; set; }
    private Dictionary<int, Data> towerDatas = new Dictionary<int, Data>();
    public event Action<int> OnChangeTowerData;
     
    [Serializable]
    public class Data : JsonSerialized
    {
        public int TowerId;
        public float OptionValue;
        public bool Unlock;
        public int TowerPartCount;
        public int grade;

        public Data()
        {
            TowerId = -1;
            OptionValue = 0f;
            Unlock = false;
            TowerPartCount = 0;
            grade = 1;
        }

        public Data(int towerId)
        {
            TowerId = towerId;
            OptionValue = 0f;
            Unlock = false;
            TowerPartCount = 0;
            grade = 1;
        }
    }

    public Data Get(int towerId)
    {
        if (towerDatas.TryGetValue(towerId, out var data))
        {
            Data copyData = new Data()
            {
                TowerId = data.TowerId,
                OptionValue = data.OptionValue,
                Unlock = data.Unlock,
                TowerPartCount = data.TowerPartCount
            };
            return copyData;
        } 
        return null;
    }

    public async UniTask AddPartCountAsync(Data towerData , int amount)
    {
        towerData.TowerPartCount += amount;
        await Save(towerData);
    }

    public async UniTask UnlockAsync(Data towerData , float optionValue)
    {
        towerData.Unlock = true;
        towerData.OptionValue = optionValue;
        await Save(towerData);
    }

    public async UniTask UpdateOptionValueAsync(Data towerData , float optionValue)
    {
        towerData.OptionValue = optionValue;
        await Save(towerData);
    }

    public async UniTask<(bool success, string msg)> LoadAsync()
    {
        await UniTask.WaitUntil(() => FirebaseManager.Instance.UserData != null);
        await UniTask.WaitUntil(() => FirebaseManager.Instance.UserId != string.Empty);

        var path = DataBasePaths.TowerPath + FirebaseManager.Instance.UserId + "/";

        var userTowerDatas = await FirebaseManager.Instance.Database.GetDatas<Data>(path);

        if(userTowerDatas.success) 
        {
            if (FirebaseManager.Instance.ChangeVersion)
            {
                // 버전 변경 시 기존 데이터 초기화
                var allTowerData = DataTableManager.TowerTable.GetAll();
                foreach (var tower in allTowerData)
                {
                    if (!userTowerDatas.data.Any(x => x.TowerId == tower.ID))
                    {
                        Data newTowerData = new Data(tower.ID);
                        string newPath = path + tower.ID;
                        var sucssess = await FirebaseManager.Instance.Database.OverwriteJsonData<Data>(newPath, newTowerData);

                        userTowerDatas.data.Add(newTowerData);
                    }
                }
            }

            foreach(var data in userTowerDatas.data)
            {
                Math.Round(data.OptionValue, 2);
                towerDatas.Add(data.TowerId, data);
            }
        }
        else
        {
            var allTowerData = DataTableManager.TowerTable.GetAll();

            foreach(var tower in allTowerData)
            {
                Data newTowerData = new Data(tower.ID);
                string newPath = path + tower.ID;
                var sucssess = await FirebaseManager.Instance.Database.OverwriteJsonData<Data>(newPath, newTowerData);
#if DEBUG_MODE
                if(sucssess)
                {
                    Debug.Log($"{DataTableManager.StringTable.Get(newTowerData.TowerId)} 저장 완료");
                }
#endif
                towerDatas.Add(newTowerData.TowerId, newTowerData);
            }
        }

        Init = true;
        return (true, "타워 데이터 로드 완료");
    }

    public async UniTask<(bool success, string msg)> Save(Data changedData)
    {
        if (!towerDatas.ContainsKey(changedData.TowerId))
        {
            return (false, "존재하지 않는 타워 ID입니다.");
        }

        towerDatas[changedData.TowerId] = changedData;

        var path = DataBasePaths.TowerPath + FirebaseManager.Instance.UserId + $"/{changedData.TowerId}";
        var success = await FirebaseManager.Instance.Database.OverwriteJsonData(path, changedData);
        if (success)
        {
            OnChangeTowerData?.Invoke(changedData.TowerId);
            return (true, "타워 데이터 저장 완료");
        }

        return (false, "타워 데이터 저장 실패");
    }

    public async UniTask<(bool success, string msg)> UnlockTower(int towerId)
    {
        if (!towerDatas.TryGetValue(towerId, out var data))
        {
            return (false, "존재하지 않는 타워 ID입니다.");
        }

        data.Unlock = true;
        return await Save(data);
    }

    public async UniTask<(bool success, string msg)> UpdateOptionValue(int towerId, float optionValue)
    {
        if (!towerDatas.TryGetValue(towerId, out var data))
        {
            return (false, "존재하지 않는 타워 ID입니다.");
        }

        data.OptionValue = optionValue;
        return await Save(data);
    }

    public bool IsUnlocked(int towerId)
    {
        if (towerDatas.TryGetValue(towerId, out var data))
        {
            return data.Unlock;
        }
        return false;
    }

    public float GetOptionValue(int towerId)
    {
        if (towerDatas.TryGetValue(towerId, out var data))
        {
            return data.OptionValue;
        }
        return 0f;
    }

    public int Count()
    {
        return towerDatas.Count;
    }

    public async UniTask WaitForInitializeAsync()
    {
        await UniTask.WaitUntil(() => Init);
    }

    public void Release()
    {
        towerDatas.Clear();
        Init = false;
    }
}
