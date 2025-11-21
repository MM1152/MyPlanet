
using System;
using UnityEngine;

/// <summary>
/// 파이어베이스에 저장되는 요소들 모두 상속받아서 사용
/// </summary>
[Serializable]
public class JsonSerialized
{
    public string PushId;
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
    public static T FromJson<T>(string json) where T : JsonSerialized
    {
        return JsonUtility.FromJson<T>(json);
    }
}

