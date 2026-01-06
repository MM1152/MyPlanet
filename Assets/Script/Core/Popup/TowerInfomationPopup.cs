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
    
        string optionString = "";

        if(towerData.Option == 2)
        {
            optionString = $"양쪽 {towerData.Option_Range}칸";
        }
        else if(towerData.Option == 0)
        {
            optionString = $"왼쪽 {towerData.Option_Range}칸";
        }
        else if(towerData.Option == 1)
        {
            optionString = $"오른쪽 {towerData.Option_Range}칸";
        }

        optionString += RandomOptionData.optionTable[towerData.Option].option.GetOptionStringFormatting() + " " + FirebaseManager.Instance.TowerData.GetOptionValue(towerData.ID) + "%";

        towerOptionText.text = optionString;
            //randomOptionData.GetData(towerData.Option).option.GetOptionStringFormatting();

        towerAttackTypeImage.sprite = towerData.AttackTypeImage;
        towerTypeImage.sprite = towerData.TypeImage;
        towerElementImage.sprite = towerData.ElementImage;
        
        if(towerData is TowerTable.UtilTower utilsTower)
        {
            towerAttackTypeImage.gameObject.SetActive(false);
            towerElementImage.gameObject.SetActive(false);

            towerAttackType.text = "";
            towerElementText.text = "";
        }
        else
        {
            towerAttackTypeImage.gameObject.SetActive(true);
            towerElementImage.gameObject.SetActive(true);
        }
    }
}