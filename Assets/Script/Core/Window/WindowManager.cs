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
            Variable.IsJoyStickActive = false;
            Open(startWindow);
            cureentWindow = windowTable[(int)startWindow];
        }
    }

    public void Open(WindowIds id)
    {
        Variable.IsJoyStickActive = false;
        cureentWindow?.Close();
        cureentWindow = windowTable[(int)id];
        cureentWindow.Open();
    }

    public void Close()
    {
        Variable.IsJoyStickActive = true;
        cureentWindow?.Close();
        cureentWindow = null;
    }
}
