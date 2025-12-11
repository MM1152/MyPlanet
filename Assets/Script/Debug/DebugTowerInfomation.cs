using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DebugTowerInfomation : MonoBehaviour
{
    public TextMeshProUGUI towerNameText;
    public DebugTowerStatus debugTowerStatus;
    public Transform debugTowerStatusRoot;
    private List<DebugTowerStatus> towerStatus = new List<DebugTowerStatus>();

    public DebugTowerManager towerManager;
    public DebugPlaceViewer placeViewer;
    public Transform placeViewerRoot;

    public Button levelUPButton;
    public Button levelDownButton;
    public Button saveButton;

    private Tower currentTower;
    private Dictionary<(int towerId , int level),LevelUpTable.Data> levelUpTable;
    private readonly string[] towerStatusTitle = new string[]
    {
        "공격 속도",
        "공격력",
        "추가 투사체",
        "공격 사거리",
        "가로 길이",
        "지속시간",
        "쿨타임",
        "추가 연사",
        "추가 필렛",
        "투사체 거리",
        "투사체 갯수",
        "폭팔 범위",
        "추가 타겟팅",
        "느려짐",
        "총알속도 느려짐",
        "기절 시간",
        "각도",
        "총알 속도",
    };

    private void Start()
    {
        levelUpTable = DataTableManager.LevelUpTable.GetAllDataToDeepCopy();
        for (int i = 0; i < towerStatusTitle.Length; i++)
        {
            var debug = Instantiate(debugTowerStatus, debugTowerStatusRoot);
            int idx = i;
            debug.Init(idx);
            debug.UpdateTitle(towerStatusTitle[i]);
            debug.onChangeValue += OnValueChangeToInputField;
            towerStatus.Add(debug);
        }

        var towerDatas = towerManager.GetAllTower();
        for(int i = 0; i < towerDatas.Count; i++)
        {
            var viewer = Instantiate(placeViewer, placeViewerRoot);
            viewer.Init(towerDatas[i]);
            viewer.callback += OnClickTowerViewerButton;
        }

        levelUPButton.onClick.AddListener(OnClickLevelUpButton);
        levelDownButton.onClick.AddListener(OnClickLevelDownButton);
#if UNITY_EDITOR
        saveButton.onClick.AddListener(() =>
        {
            DataTableManager.LevelUpTable.SaveData(DataTableIds.LevelUpTable, levelUpTable.Values.ToList()).Forget();
        });
#endif
    }

    private void OnClickLevelUpButton()
    {
        if(currentTower == null)
            return;

        towerManager.LevelUpTower(currentTower.TowerData);
        UpdateTexts(currentTower);
    }

    private void OnClickLevelDownButton()
    {
        if(currentTower == null)
            return;

        towerManager.LevelDownTower(currentTower.TowerData);
        UpdateTexts(currentTower);
    }

    private void UpdateTexts(Tower tower)
    {

        float[] datas = new float[]
        {
            currentTower.BonusAttackSpeed,
            currentTower.BonusDamage,
            currentTower.BonusProjectileCount,
            currentTower.BonusAttackRange,
            currentTower.BonusWidthSize,
            currentTower.BonusDuration,
            currentTower.BonusCoolTime,
            currentTower.BonusFireRate,
            currentTower.BonusPelletCount,
            currentTower.BonusFregmentRange,
            currentTower.BonusFregmentCount,
            currentTower.BonusExplosionRange,
            currentTower.BonusTargetingCount,
            currentTower.BonusSlowPercent,
            currentTower.BonusSlowBulletSpeed,
            currentTower.BonusStopTime,
            currentTower.BonuseNoise,
            currentTower.BonusBulletSpeed,
        };

        for (int i = 0; i < towerStatus.Count; i++)
        {
            towerStatus[i].UpdateInputField(datas[i]);
        }
        towerNameText.text = currentTower.TowerData.Name + $" +{tower.Level}";

    }

    private void OnClickTowerViewerButton(Tower tower)
    {
        if (currentTower != null)
            currentTower.UnPlaceTower();

        currentTower = tower;
        currentTower.PlaceTower();

        UpdateTexts(currentTower);
    }

    private void OnValueChangeToInputField(int index , float value)
    {
        if (currentTower == null) return;
        var levelUpData = levelUpTable[(currentTower.ID, currentTower.Level)];

        if(index != 0 && index != 1)
        {
            if (levelUpData.Var1 == index - 1) levelUpData.Val1 = (int)value;
            else if (levelUpData.Var2 == index - 1) levelUpData.Val2 = (int)value;
            else if (levelUpData.Var3 == index - 1) levelUpData.Val3 = (int)value;
            else if (levelUpData.Var4 == index - 1) levelUpData.Val4 = (int)value;
        }


        switch (index)
        {
            case 0:
                currentTower.BonusAttackSpeed = value;
                break;
            case 1:
                currentTower.BonusDamage = (int)value;
                levelUpData.Damage = (int)value;
                break;
            case 2:
                currentTower.BonusProjectileCount = (int)value;
                break;
            case 3:
                currentTower.BonusAttackRange = (int)value;
                break;
            case 4:
                currentTower.BonusWidthSize = (int)value;
                break;
            case 5:
                currentTower.BonusDuration = (int)value;
                break;
            case 6:
                currentTower.BonusCoolTime = (int)value;
                break;
            case 7:
                currentTower.BonusFireRate = (int)value;
                break;
            case 8:
                currentTower.BonusPelletCount = (int)value;
                break;
            case 9:
                currentTower.BonusFregmentRange = (int)value;
                break;
            case 10:
                currentTower.BonusFregmentCount = (int)value;
                break;
            case 11:
                currentTower.BonusExplosionRange = (int)value;
                break;
            case 12:
                currentTower.BonusTargetingCount = (int)value;
                break;
            case 13:
                currentTower.BonusSlowPercent = (int)value;
                break;
            case 14:
                currentTower.BonusSlowBulletSpeed = (int)value;
                break;
            case 15:
                currentTower.BonusStopTime = (int)value;
                break;
            case 16:
                currentTower.BonuseNoise = (int)value;
                break;
            case 17:
                currentTower.BonusBulletSpeed = (int)value;
                break;
        }
    }
}
