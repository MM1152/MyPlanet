using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PresetData
{
    public bool Init { get; set; }
    private List<Data> presetDatas = new List<Data>();
    private Data inGameData;
    public event Action<int> OnChangePresetData;

    [Serializable]
    public class Data : JsonSerialized
    {
        public string PresetName;
        public int PlanetId;
        public List<int> TowerId;
        public Data()
        {
            PresetName = "프리셋";
            PlanetId = 1001;
            TowerId = new List<int>() { -1, -1, -1, -1, -1 , -1 , -1 ,-1 , -1, -1 ,-1 ,-1};
        }
    }

    public void SetGameData(Data InGameData)
    {
        this.inGameData = InGameData;
    } 

    public Data GetGameData()
    {
        return inGameData;
    }

    public Data Get(int index)
    {
        Data copyData = new Data()
        {
            PresetName = presetDatas[index].PresetName,
            PlanetId = presetDatas[index].PlanetId,
            TowerId = new List<int>(presetDatas[index].TowerId),
            PushId = presetDatas[index].PushId
        };
        return copyData;
    }

    public async UniTask<(bool sucess , string msg)> LoadAsync()
    {
        await UniTask.WaitUntil(() => FirebaseManager.Instance.UserData != null);
        await UniTask.WaitUntil(() => FirebaseManager.Instance.UserId != string.Empty);

        var path = DataBasePaths.PresetPath + FirebaseManager.Instance.UserId + "/";
        var result = await FirebaseManager.Instance.Database.GetDatas<Data>(path);

        if(result.success)
        {
            // 이전에 로그인을 해서 프리셋 데이터를 가지고 있는 경우
            if(FirebaseManager.Instance.ChangeVersion)
            {
                for(int i = 0; i < result.data.Count; i++)
                {
                    var newPath = path + result.data[i].PushId;
                    var success = await FirebaseManager.Instance.Database.OverwriteJsonData(newPath, result.data[i]);
                }
            }

            foreach(var data in result.data)
            {
                presetDatas.Add(data);
            }
            Init = true;
            Debug.Log("프리셋 데이터 로드 완료");
            return (true, "프리셋 데이터 로드 완료");
        }
        else
        {
            // 처음 로그인해서 프리셋 데이터가 없는 경우
            for(int i = 0; i < 10; i++)
            {
                Data newPresetData = new Data();
                presetDatas.Add(newPresetData);
                var success = await FirebaseManager.Instance.Database.PushWirteJsonData(path, newPresetData);
                if (success)
                {
                    Debug.Log("초기 프리셋 데이터 만들기 완료");
                }
            }
            
            Init = true;
            return (true, "프리셋 데이터 로드 성공");
        }
    }

    public async UniTask<(bool sucess , string msg)> Save(Data changedData , int index)
    {
        presetDatas[index] = changedData;

        var path = DataBasePaths.PresetPath + FirebaseManager.Instance.UserId + $"/{changedData.PushId}";
        var success = await FirebaseManager.Instance.Database.OverwriteJsonData(path, changedData);
        if(success)
        {
            OnChangePresetData?.Invoke(index);
            return (true, "프리셋 데이터 저장 완료");
        }

        return (false, "프리셋 데이터 저장 실패");
    }

    public int Count()
    {
        return presetDatas.Count;
    }

    public async UniTask WaitForInitalizeAsync()
    {
        await UniTask.WaitUntil(() => Init);
    }

    public void Release()
    {
        presetDatas.Clear();
        inGameData = null;
        Init = false;
    }
}

