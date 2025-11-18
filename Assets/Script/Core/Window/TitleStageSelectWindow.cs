using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class TitleStageSelectWindow : Window
{
    [SerializeField] private Button backButton;    
    [SerializeField] private Button selectButton;    
    public override void Close()
    {
        base.Close();
    }

    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        windowId = (int)WindowIds.TitleStageSelectedWindow;

        backButton.onClick.AddListener(() => manager.Open(WindowIds.TitleMainWindow));
        selectButton.onClick.AddListener(() => manager.Open(WindowIds.TitlePresetWindow));
    }

    public override void Open()
    {
        base.Open();
    }
}
