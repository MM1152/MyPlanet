using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerInfomationPopup : Popup
{
    [SerializeField] private TextMeshProUGUI towerNameText;
    [SerializeField] private TextMeshProUGUI towerType;
    [SerializeField] private TextMeshProUGUI towerAttackType;
    [SerializeField] private TextMeshProUGUI towerElementText;
    [SerializeField] private TextMeshProUGUI towerDescriptionText;
    [SerializeField] private TextMeshProUGUI towerOptionText;

    [SerializeField] private TextMeshProUGUI towerATKText;
    [SerializeField] private TextMeshProUGUI towerFireRateText;
    [SerializeField] private TextMeshProUGUI towerRangeText;

    [SerializeField] private Image towerImage;
    [SerializeField] private Image towerTypeImage;
    [SerializeField] private Image towerAttackTypeImage;
    [SerializeField] private Image towerElementImage;

    //[SerializeField] private RandomOptionData randomOptionData = new RandomOptionData(); 
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
        towerImage.sprite = towerData.towerImage;
        //towerGradeText.text = towerData.grade ??
        towerType.text = towerData.TypeToString;
        towerAttackType.text = towerData.AttackTypeToString;
        towerElementText.text = (towerData.AttributeToString).ToString();
        towerATKText.text = towerData.ATK.ToString();
        towerFireRateText.text = towerData.Fire_Rate.ToString();
        towerRangeText.text = towerData.Attack_Range.ToString();
        towerDescriptionText.text = towerData.Explanatoin;
        towerOptionText.text = towerData.Buff_Explanation;
            //randomOptionData.GetData(towerData.Option).option.GetOptionStringFormatting();

        towerAttackTypeImage.sprite = DataTableManager.SpriteTable.Get(DataTableIds.AttackTypeSpriteTable , towerData.ATK_Type);
        towerTypeImage.sprite = DataTableManager.SpriteTable.Get(DataTableIds.TypeSpriteTable , towerData.Type);
        towerElementImage.sprite = DataTableManager.SpriteTable.Get(DataTableIds.ElementSpriteTable, towerData.attribute);
        
    }
}