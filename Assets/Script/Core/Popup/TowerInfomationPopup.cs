using TMPro;
using UnityEngine;

public class TowerInfomationPopup : Popup
{
    private readonly string towerName = "타워 이름 : {0}";
    private readonly string towerDamage = "타워 공격력 : {0}";
    private readonly string towerAttackSpeed = "타워 공격속도 : {0}";
    private readonly string towerAttackRange = "타워 공격거리 : {0}";
    private readonly string towerRandomOption = "타워 랜덤옵션 : {0}";

    [SerializeField] private TextMeshProUGUI towerNameText;
    [SerializeField] private TextMeshProUGUI towerDamageText;
    [SerializeField] private TextMeshProUGUI towerAttackSpeedText;
    [SerializeField] private TextMeshProUGUI towerAttackRangeText;
    [SerializeField] private TextMeshProUGUI towerRandomOptionText;

    public override bool Close()
    {
        return base.Close();
    }

    public override void Init(PopupManager manager)
    {
        base.Init(manager);
    }

    public override void Open()
    {
        base.Open();
    }

    public void UpdateTexts(Tower tower)
    {
        towerNameText.text = string.Format(towerName, tower.ID);
        towerDamageText.text = string.Format(towerDamage, tower.Damage); 
        towerAttackSpeedText.text = string.Format(towerAttackSpeed, tower.AttackSpeed);
        towerAttackRangeText.text = string.Format(towerAttackRange, tower.AttackRange);
        towerRandomOptionText.text = string.Format(towerRandomOption, tower.Option.GetOptionStringFormatting());
    }
}