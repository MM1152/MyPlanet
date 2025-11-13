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

    private TouchManager touchManager;
       
    private void Init()
    {
        var touchManager = new GameObject("TouchManager");
        this.touchManager = touchManager.AddComponent<TouchManager>();
        this.touchManager.Init();
    }
}