using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class WindowManager : MonoBehaviour
{
    [SerializeField] private List<Window> windows;
    [SerializeField] private WindowIds startWindow;
    [SerializeField] private Window cureentWindow;
    
    private Dictionary<int, Window> windowTable = new Dictionary<int, Window>();


    private void Start()
    {
        foreach (var window in windows)
        {
            window.Init(this);
            windowTable.Add(window.WindowId, window);
            window.Close();
        }

        if(windowTable.ContainsKey((int)startWindow)) {
            windowTable[(int)startWindow].Open();
            cureentWindow = windowTable[(int)startWindow];
        }
    }

    public void Open(WindowIds id)
    {
        cureentWindow?.Close();
        cureentWindow = windowTable[(int)id];
        cureentWindow.Open();
    }

    public void Close()
    {
        cureentWindow?.Close();
        cureentWindow = null;
    }
}
