using UnityEngine;
using System.Collections.Generic;

public class WindowManager : MonoBehaviour
{
    [SerializeField] private List<Window> windows;
    [SerializeField] private WindowIds startWindow;

    private Dictionary<int, Window> windowTable = new Dictionary<int, Window>();

    private void Start()
    {
        foreach(var window in windows)
        {
            window.Init();
            windowTable.Add(window.WindowId, window);
            window.Close();
        }

        if(windowTable.ContainsKey((int)startWindow)) {
            windowTable[(int)startWindow].Open();
        }
    }
}
