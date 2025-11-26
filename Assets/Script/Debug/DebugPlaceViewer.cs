using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DebugPlaceViewer : MonoBehaviour
{
    public Tower tower;
    public DebugTowerManager towerManager;

    public TextMeshProUGUI towerNameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI towerPlaceAbleText;
    public Button towerPlaceButton;
    public Button towerLevelUpButton;

    public void Init(DebugTowerManager towerManager, Tower tower)
    {
        this.towerManager = towerManager;
        this.tower = tower;
        UpdateText();

        towerPlaceButton.onClick.AddListener(OnClickPlaceButton);
        towerLevelUpButton.onClick.AddListener(OnClickLevelUpButton);
    }

    public void UpdateText()
    {
        towerNameText.text = tower.TowerData.Name;
        levelText.text = $"Level: {tower.Level}";
        towerPlaceAbleText.text = tower.UseAble ? "설치 취소" : "설치";
    }

    private void OnClickPlaceButton()
    {
        if(tower.UseAble)
        {
            towerManager.UnPlaceTower(tower.TowerData);
        }
        else
        {
            towerManager.PlaceTower(tower.TowerData);
        }
        UpdateText();
    }

    private void OnClickLevelUpButton()
    {
        if (tower.Level >= 5) return;
        towerManager.LevelUpTower(tower.TowerData);

        UpdateText();
    }
}
