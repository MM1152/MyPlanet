using TMPro;
using UnityEngine;

public class StatusViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI nameText;
    private Tower tower;
    private BasePlanet planet;

    public void Init(Tower tower , BasePlanet planet)
    {
        this.tower = tower;
        this.planet = planet;

    }

    public void UpdateStatus()
    {
        nameText.text = $"{tower.SlotIndex} ¹ø Å¸¿ö";
        statusText.text = $"{tower.BaseDamage} + {tower.BonusDamage}\n";
        statusText.text += $"{tower.BaseAttackSpeed} + {tower.BonusAttackSpeed}\n";
        statusText.text += $"0 + 0\n";
        statusText.text += $"0 + 0\n";
    }

    public void UpdatePlanetText()
    {
    }
}
