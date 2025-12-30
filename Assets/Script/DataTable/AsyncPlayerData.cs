using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System;

public class AsyncPlayerData
{
    public class Data : JsonSerialized
    {
        public string playerNickName;
        public int playerPlanetId;
        public string imageUrl;
        public List<int> playerTowerIds;
        public List<int> playerTowerLevels;
        public List<int> playerTowerFullDamages;
    }

    public async UniTask SaveAsyncData(int stageId , Data asyncPlayerData)
    {
        var dataRefPath = string.Format(DataBasePaths.AsyncPlayerSavePathFormating, stageId);

        bool success = await FirebaseManager.Instance.Database.OverwriteJsonData<Data>(dataRefPath, asyncPlayerData);
        if(success)
        {
            Debug.Log("비동기 데이터 저장 완료");       
        }
    }

    public async UniTask<(bool isSuccess , List<Data> datas)> LoadAsyncData(int stageId)
    {
        var dataRefPath = string.Format(DataBasePaths.AsyncStagePathFormating, stageId);
        var (data, success) = await FirebaseManager.Instance.Database.GetDatas<Data>(dataRefPath);
        Utils.Suffle(data);
        if(success)
        {
            Debug.Log("비동기 데이터 로드 완료");
            return (true, data);
        }
        return (false, null);
    }

}
