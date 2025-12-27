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
    [SerializeField] private TextMeshProUGUI diamondText;


    public Button ExitButton => exitButton;
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
        FirebaseManager.Instance.Database.AddListner(DataBasePaths.DiamondPath, OnValueChangeToDiamond);
    }

    public void OnDestroy()
    {
        FirebaseManager.Instance.Database.RemoveListner(DataBasePaths.GoldPath, OnValueChangeToGold);
        FirebaseManager.Instance.Database.RemoveListner(DataBasePaths.ExpPath, OnValueChangeToExp);
        FirebaseManager.Instance.Database.RemoveListner(DataBasePaths.DiamondPath, OnValueChangeToDiamond);
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
        goldText.text = int.Parse(args.Snapshot.Value.ToString()).ToString("N0");
    }

    private void OnValueChangeToExp(object sender, ValueChangedEventArgs args)
    {
        expText.text = int.Parse(args.Snapshot.Value.ToString()).ToString("N0");
    }

    private void OnValueChangeToDiamond(object sender, ValueChangedEventArgs args)
    {
        diamondText.text = int.Parse(args.Snapshot.Value.ToString()).ToString("N0");
    }

}
