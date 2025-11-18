using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class WindowManager : MonoBehaviour
{
    [Header("Window Settings")]
    [SerializeField] private List<Window> windows;
    [SerializeField] private WindowIds startWindow;
    [SerializeField] private Window cureentWindow;
    [Header("Button Settings")]
    [SerializeField] private Button openStatusViewButton;

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

        if(openStatusViewButton != null)
            openStatusViewButton.onClick.AddListener(() => Open(WindowIds.StatusWindow));
    }

    public Window Open(WindowIds id)
    {
        Variable.IsJoyStickActive = false;
        cureentWindow?.Close();
        cureentWindow = windowTable[(int)id];
        cureentWindow.Open();

        return cureentWindow;
    }

    public void Close()
    {
        Variable.IsJoyStickActive = true;
        cureentWindow?.Close();
        cureentWindow = null;
    }
}
