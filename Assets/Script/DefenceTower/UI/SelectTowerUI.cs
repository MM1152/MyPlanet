using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SelectTowerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI towerNameText; //타워이름
    [SerializeField] private TextMeshProUGUI slotIndexText; //슬롯위치넘버 
    [SerializeField] private TextMeshProUGUI towerDescriptionText; //타워 간략설명
    [SerializeField] private TextMeshProUGUI towerState; //타워 능력 상태 

    [Header("UI Elements")]
    [SerializeField] private Image backGroundImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private Image towerImage; // 선택된 타워 배경 이미지

    [Header("Tower UI")]
    [SerializeField] private Sprite towerBackGroundImage; // 배경 이미지 
    [SerializeField] private Sprite towerIconBackgroundImage; // 아이콘 배경 이미지
    [SerializeField] private Sprite towerChoiceBackgroundImage; // 선택된 타워 배경 이미지

    [Header("Consumable UI")]
    [SerializeField] private Sprite consumableBackgroundImage; // 소모품 배경 이미지
    [SerializeField] private Sprite consumableIconImage; // 소모품 아이콘 이미지
    [SerializeField] private Sprite consumableChoiceBackgroundImage; // 선택된 소모품 배경 이미지

    [Header("Toggle")]
    [SerializeField] private Toggle toggle;
    [SerializeField] private ToggleGroup toggleGroup;

    private TowerTable.Data towerData;
    private ConsumalbeTable.Data consumeData;
    private bool isTower;
    

    private List<string> upgradeList = new List<string>();
    private System.Text.StringBuilder sb = new System.Text.StringBuilder();

    private Action<int> OnChangeIndex;

    public void Initalized(int index, Action<int> callback)
    {
        if (toggleGroup != null)
        {
            toggle.group = toggleGroup;
        }
     
        OnChangeIndex = callback;
        toggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                Managers.SoundManager.PlaySFX(AudiosId.ui_button_simple_click_06);
                UpdateBackgroundImage(true);
                OnChangeIndex?.Invoke(index);
            }
            else
            {
                UpdateBackgroundImage(false);
                OnChangeIndex?.Invoke(-1);  
            }
        });
    }

    public void SetInteractive(bool active)
    {
        toggle.interactable = active;
    }

    public void SetTowerData(Tower data)
    {
        gameObject.SetActive(true);
        isTower = true;
        this.towerData = data.TowerData;
        towerNameText.text = $"{towerData.Name}" + (data.Level == 0 ? "" : $"+{data.Level}단계");
        slotIndexText.text = data.SlotIndex + "번 슬롯";
        towerState.gameObject.SetActive(true);
        iconImage.sprite = towerIconBackgroundImage;
        towerImage.sprite = data.TowerData.towerImage;
        var currentLevelData = DataTableManager.LevelUpTable.Get(data.TowerData.ID, data.Level);
        var nextLevelData = DataTableManager.LevelUpTable.Get(data.TowerData.ID, data.Level + 1);
        
        if (currentLevelData != null)
            Debug.Log($"CurrentLevel {data.Level} - Var1:{currentLevelData.Var1} Val1:{currentLevelData.Val1}");
        if (nextLevelData != null)
            Debug.Log($"NextLevel {data.Level + 1} - Var1:{nextLevelData.Var1} Val1:{nextLevelData.Val1}");

        if (nextLevelData == null) 
        {
          Debug.Log("최대레벨 도달");
        }
        else
        {
            upgradeList.Clear();
            sb.Clear();

            int currentDamage = currentLevelData?.Damage ?? 0;
            int damageDiff = nextLevelData.Damage - currentDamage;
            if (damageDiff > 0)
            {
                upgradeList.Add($"[공격력] +{damageDiff}");
            }

            CheckVarUpgrade(currentLevelData?.Var1 ?? 0, currentLevelData?.Val1 ?? 0, nextLevelData.Var1, nextLevelData.Val1);
            CheckVarUpgrade(currentLevelData?.Var2 ?? 0, currentLevelData?.Val2 ?? 0, nextLevelData.Var2, nextLevelData.Val2);
            CheckVarUpgrade(currentLevelData?.Var3 ?? 0, currentLevelData?.Val3 ?? 0, nextLevelData.Var3, nextLevelData.Val3);
            CheckVarUpgrade(currentLevelData?.Var4 ?? 0, currentLevelData?.Val4 ?? 0, nextLevelData.Var4, nextLevelData.Val4);

            if (upgradeList.Count > 0)
            {
                for (int i = 0; i < upgradeList.Count; i++)
                {
                    if (i > 0 && i % 2 == 0) sb.Append("\n");
                    else if (i > 0) sb.Append(" / ");
                    sb.Append(upgradeList[i]);
                }
                towerState.text = $"{sb.ToString()}";
            }
            else
            {
                towerState.text = "능력치 변화 없음";
            }
        }

        UpdateBackgroundImage(false);
    }

    private void CheckVarUpgrade(int currentVar, float currentVal, int nextVar, float nextVal)
    {
        if (nextVar <= 0) return;

        float compareVal = (currentVar == nextVar) ? currentVal : 0;
        float diff = nextVal - compareVal;

        if (diff == 0) return;

        string effectName = LevelUpEffectDescriptions.GetLevelUpEffectDescription(nextVar);
        if (string.IsNullOrEmpty(effectName)) return;
  
        // 쿨타임(Var5)만 감소 표시, 다른 능력치는 증가만 표시
        if (nextVar == 5)
        {
            if (diff < 0)
            {
                upgradeList.Add($"[{effectName}] -{Mathf.Abs(diff):F0}");
            }
        }
        else if (diff > 0)
        {
            upgradeList.Add(diff % 1 == 0 ? $"[{effectName}] +{Mathf.Abs(diff):F0}" : $"[{effectName}] +{Mathf.Abs(diff):F1}");
        }
    }

    public void SetConsumableData(ConsumalbeTable.Data data)
    {
        gameObject.SetActive(true);
        isTower = false;
        this.consumeData = data;
        towerNameText.text = data.Name;
        slotIndexText.text = "소모품";
        towerState.text = data.Description;
        iconImage.sprite = consumableIconImage;
        towerImage.sprite = data.consumableImage;

        UpdateBackgroundImage(false);
    }

    private void UpdateBackgroundImage(bool isSelected)
    {
        if (isTower)
        {
            backGroundImage.sprite = isSelected ? towerChoiceBackgroundImage : towerBackGroundImage;
        }
        else
        {
            backGroundImage.sprite = isSelected ? consumableChoiceBackgroundImage : consumableBackgroundImage;
        }
    }

    public TowerTable.Data GetTowerData()
    {
        return towerData;
    }

    public ConsumalbeTable.Data GetCosumaableData()
    {
        return consumeData;
    }

    public void ResetOutline()
    {
        towerData = null;
        consumeData = null;
        toggle.isOn = false;
    }

    public Toggle GetToggle()
    {
        return toggle;
    }
}
