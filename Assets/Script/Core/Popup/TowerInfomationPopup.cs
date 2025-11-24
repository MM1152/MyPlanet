using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerInfomationPopup : Popup
{
    [SerializeField] private TextMeshProUGUI towerNameText;
    [SerializeField] private TextMeshProUGUI towerGradeText;
    [SerializeField] private TextMeshProUGUI towerTypeText;
    [SerializeField] private TextMeshProUGUI towerElementText;
    [SerializeField] private TextMeshProUGUI towerDescriptionText;

    [SerializeField] private Image towerImage;

    public override bool Close()
    {
        return base.Close();
    }

    public override void Init(PopupManager manager)
    {
        base.Init(manager);
        popupId = (int)PopupIds.TowerInfomationPopup;
    }

    public override void Open()
    {
        base.Open();
    }

    public void UpdateTexts(TowerTable.Data towerData)
    {
        towerNameText.text = towerData.Name;
        //towerGradeText.text = towerData.grade ??
        towerTypeText.text = towerData.AttackType;
        towerElementText.text = ((ElementType)towerData.Attribute).ToString();
        //towerDescriptionText.text = towerData. ??
    }
}