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
    [SerializeField] private Button pauseViewButton;

    private Dictionary<int, Window> windowTable = new Dictionary<int, Window>();
    

    private void Awake()
    {
        foreach (var window in windows)
        {
            window.Init(this);
            windowTable.Add(window.WindowId, window);
            window.Close();
        }

        if(windowTable.ContainsKey((int)startWindow)) {
            if(!Variable.IsTutorialActive)
            {
                Variable.IsJoyStickActive = false;
                Open(startWindow);
                cureentWindow = windowTable[(int)startWindow];
            }
        }

        if(pauseViewButton != null)
            pauseViewButton.onClick.AddListener(() => Open(WindowIds.PauseWindow));

        if(openStatusViewButton != null)
            openStatusViewButton.onClick.AddListener(() => Open(WindowIds.StatusWindow));

        if(Variable.IsTutorialActive)
        {
            Time.timeScale = 0f;
        }
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
        Time.timeScale = (int)GameSpeed.CurrentSpeed;
    }

    public Window GetWindow(WindowIds windowId)
    {
        return windowTable[(int)windowId];
    }
}
