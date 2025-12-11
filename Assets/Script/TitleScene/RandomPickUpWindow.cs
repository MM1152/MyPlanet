using UnityEngine;
using UnityEngine.UI;

public class RandomPickUpWindow : Window
{
    [SerializeField] private RandomPickUpLayout randomPickUpLayoutForPlanet;
    [SerializeField] private Button backButton;
    public override void Close()
    {
        base.Close();
    }

    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        windowId = (int)WindowIds.RandomPickUpWindow;

        randomPickUpLayoutForPlanet.Init(DataTableManager.RandomPickUpTable.GetAllDataForPlanet());
        backButton.onClick.AddListener(() => manager.Open(WindowIds.TitleMainWindow));
    }

    public override void Open()
    {
        base.Open();
    }
}
