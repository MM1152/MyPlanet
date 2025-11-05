
using UnityEngine;

public class JsonSerialized
{
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
    public static T FromJson<T>(string json) where T : JsonSerialized
    {
        return JsonUtility.FromJson<T>(json);
    }
}

