using System.Text;
using TMPro;
using UnityEngine;

public class StatusViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI statusTitle;
    [SerializeField] private TextMeshProUGUI nameText;
    private Tower tower;
    private BasePlanet planet;
    private StringBuilder sb = new StringBuilder();
    public void Init(Tower tower , BasePlanet planet)
    {
        this.tower = tower;
        this.planet = planet;
    }

    public void UpdateStatus()
    {
        sb.Clear();
        sb.AppendLine($"{tower.BaseDamage} + {tower.BonusDamage} + {tower.BonusDamagePercent * 100f}%");
        sb.AppendLine($"{tower.BaseAttackSpeed} + {tower.BonusAttackSpeed} + {tower.BonusAttackSpeedPercent * 100f}%");
        sb.AppendLine($"{tower.BaseAttackRange} + {tower.BonusAttackRange}");
        statusText.text = sb.ToString();

        sb.Clear();
        sb.AppendLine($"공격력");
        sb.AppendLine($"공격속도");
        sb.AppendLine($"공격범위");
        statusTitle.text = sb.ToString();
        nameText.text = tower.TowerData.Name;
    }

    public void UpdatePlanetText()
    {
        sb.Clear();
        sb.AppendLine($"{planet.maxHp}");
        sb.AppendLine($"{planet.FullDEF}");
        statusText.text = sb.ToString();

        nameText.text = planet.PlanetData.Name;

        sb.Clear();
        sb.AppendLine($"체력");
        sb.AppendLine($"방어력");
        statusTitle.text = sb.ToString();
    }
}
