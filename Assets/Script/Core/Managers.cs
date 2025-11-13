using Unity.VisualScripting;
using UnityEngine;

public class Managers
{
    private static Managers instance;
    public static Managers Instance
    {
        get
        {
            if( instance == null)
            {
                instance = new Managers();
                instance.Init();
            }

            return instance;
        }
    }

    public static TouchManager TouchManager => Instance.touchManager;
    public static ObjectPoolManager ObjectPoolManager => Instance.objectPoolManager;

    private TouchManager touchManager;
    private ObjectPoolManager objectPoolManager;

    private void Init()
    {
        var go = new GameObject("Managers");
        GameObject.DontDestroyOnLoad(go);

        var touchManager = new GameObject("TouchManager");
        this.touchManager = touchManager.AddComponent<TouchManager>();
        this.touchManager.Init();
        this.touchManager.transform.SetParent(go.transform);

        var objectPoolManager = new GameObject("ObjectPoolManager");
        this.objectPoolManager = objectPoolManager.AddComponent<ObjectPoolManager>();
        this.objectPoolManager.transform.SetParent(go.transform);
    }

    public void Release()
    {
        objectPoolManager.Release();
    }
}