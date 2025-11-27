using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleBookInfomationWindow : Window
{
    [Header("Buttons")]
    [SerializeField] private Button exitButton;

    [Header("Viewers")]
    [SerializeField] private PlanetInfoViewer planetInfoViewer;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI expText;

    public override void Close()
    {
        base.Close();
    }

    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        windowId = (int)WindowIds.TitleBookInfomationWindow;
        exitButton.onClick.AddListener(() => {
            manager.Open(WindowIds.TitleBookWindow);
        });

        FirebaseManager.Instance.Database.AddListner(DataBasePaths.GoldPath, OnValueChangeToGold);
        FirebaseManager.Instance.Database.AddListner(DataBasePaths.ExpPath, OnValueChangeToExp);
    }

    public override void Open()
    {
        base.Open();
    }

    public void UpdatePlanetData(PlanetTable.Data planetTableData)
    {
        planetInfoViewer.UpdatePlanetData(planetTableData);
    }

    private void OnValueChangeToGold(object sender, ValueChangedEventArgs args)
    {
        goldText.text = args.Snapshot.Value.ToString();
    }

    private void OnValueChangeToExp(object sender, ValueChangedEventArgs args)
    {
        expText.text = args.Snapshot.Value.ToString();
    }
}
