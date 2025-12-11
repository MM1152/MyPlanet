using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerInfomationWindow : Window
{
    [Header("Buttons")]
    [SerializeField] private Button closeButton;
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI towerNameText;
    [SerializeField] private TextMeshProUGUI towerTypeText;
    [SerializeField] private TextMeshProUGUI towerAttackTypeText;
    [SerializeField] private TextMeshProUGUI towerElementText;
    [SerializeField] private TextMeshProUGUI towerInfomationText;
    [SerializeField] private TextMeshProUGUI towerOptionText;
    [Header("Images")]
    [SerializeField] private Image towerImage;
    [SerializeField] private Image towerTypeImage;
    [SerializeField] private Image towerAttackTypeImage;
    [SerializeField] private Image towerElementImage;

    public override void Close()
    {
        base.Close();
    }

    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        windowId = (int)WindowIds.TowerInfomationWindow;

        closeButton.onClick.AddListener(() => { manager.Open(WindowIds.TitleBookWindow); });
    }

    public override void Open()
    {
        base.Open();
    }

    public void SettingTowerData(TowerTable.Data towerData)
    {
        towerNameText.text = towerData.Name;
        towerTypeText.text = towerData.TypeToString;
        towerAttackTypeText.text = towerData.AttackType;
        towerElementText.text = towerData.AttributeToString;
        towerInfomationText.text = towerData.Explanatoin;
        towerOptionText.text = towerData.Buff_Explanation;

        towerTypeImage.sprite = towerData.TypeImage;
        towerAttackTypeImage.sprite = towerData.AttackTypeImage;
        towerElementImage.sprite = towerData.ElementImage;
    }
}
