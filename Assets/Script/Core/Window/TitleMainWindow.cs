using UnityEngine;
using UnityEngine.UI;

public class TitleMainWindow : Window
{
    [SerializeField] private Button selectStageButton;
    public override void Close()
    {
        base.Close();
    }

    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        windowId = (int)WindowIds.TitleMainWindow;

        selectStageButton.onClick.AddListener(() => manager.Open(WindowIds.TitleStageSelectedWindow));
    }

    public override void Open()
    {
        base.Open();
    }
}
