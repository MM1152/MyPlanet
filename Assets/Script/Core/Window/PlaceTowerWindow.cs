using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PlaceTowerWindow : Window
{
    [SerializeField] private int selectTowerUICount;

    [Header("Drag To Inspector")]
    [SerializeField] private SelectTowerUI selectTowerUI; // 셀렉트타워 ㅅ크립트 
    [SerializeField] private Transform selectTowerUIRoot; // //셀렉트 타워 UI  통짜
    [SerializeField] private TowerManager towerManager;
    [SerializeField] private TextMeshProUGUI titleText; // 아이콘으로 대체됨 
    [SerializeField] private TextMeshProUGUI levelText; // 레벨텍스트 필요 
    [SerializeField] private Button selectTowerButton; // 타워 선택 버튼
    [SerializeField] private ConsumableManager consumableManager;

    [Header("Ÿ���� �Ҹ�ǰ ���������� ���� Ȯ��")]
    [SerializeField] private float towerSpawnPercent; //타워 나올 확률 
    [SerializeField] private TutorialManager tutorialManager;
    private List<SelectTowerUI> selectTowerUIs = new List<SelectTowerUI>(); //선택할 타워 UI들 
    private int selectTowerIndex = -1; //처음 선택된 타워 인덱스
    private bool isStartTutorial = false;
    private List<Tower> availableTowers = new List<Tower>(); //사용 가능한 타워들
    private List<ConsumalbeTable.Data> availableConsumables = new List<ConsumalbeTable.Data>(); //사용 가능한 소모품들

#if DEBUG_MODE
    [Header("�̰� ���� Ÿ�� ID �� �ֱ�")]
    public int towerId;
    [Header("�̰� ���� �Ҹ�ǰ ID �� �ֱ�")]
    public int consumableId;
#endif

    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        isStartTutorial = false;
        for (int i = 0; i < selectTowerUIRoot.childCount; i++)
        {
            SelectTowerUI obj = selectTowerUIRoot.GetChild(i).GetComponentInChildren<SelectTowerUI>();
            if( obj == null ) continue; 
            obj.Initalized(i, (value) => selectTowerIndex = value);
            selectTowerUIs.Add(obj);
        }

        windowId = (int)WindowIds.PlaceTowerWindow;
        selectTowerButton.onClick.AddListener(OnClickSelectTowerButton);
    }

    private void OnClickSelectTowerButton()
    {
        if (selectTowerIndex == -1) return;
        var towerData = selectTowerUIs[selectTowerIndex].GetTowerData();
        if (towerData != null)
        {
            towerManager.PlaceTower(towerData);

            if (!isStartTutorial && FirebaseManager.Instance.PresetData.GetGameData().stageId == 2 && !FirebaseManager.Instance.UserData.isClearStage2Tutorial)
            {
                isStartTutorial = true;
                tutorialManager.InitTutorial(TutorialStep.Stage2);
            }
        }
        else
        {
            var consumData = selectTowerUIs[selectTowerIndex].GetCosumaableData();
            consumableManager.SetConsumable(consumData);
        }

        manager.Close();
    }

    public override void Open()
    {
        if (Variable.IsTutorialActive) 
            return;

        selectTowerIndex = -1;
        levelText.text = $"Lv. {towerManager.CurrentLevel}";

        availableTowers.Clear();

        var allTowers = towerManager.GetAllTower();
        for (int i = 0; i < allTowers.Count; i++)
        {
            if (allTowers[i] == null) continue;

            var nextLevelData = DataTableManager.LevelUpTable.Get(
                allTowers[i].TowerData.ID,
                allTowers[i].Level + 1
            );

            if (nextLevelData != null)
            {
                availableTowers.Add(allTowers[i]);
            }
        }
            
        availableConsumables.Clear();
        var sourceConsumables = consumableManager.GetAllData();
        for (int i = 0; i < sourceConsumables.Count; i++)
        {
            availableConsumables.Add(sourceConsumables[i]);
        }

        for (int i = 0; i < selectTowerUIs.Count; i++)
        {
            selectTowerUIs[i].gameObject.SetActive(false);
        }

        for (int slotIndex = 0; slotIndex < selectTowerUICount; slotIndex++)
        {
            if (towerManager.CurrentLevel == 1 && availableTowers.Count == 0)
            {
                break;
            }

            if (availableTowers.Count == 0 && availableConsumables.Count == 0)
            {
                break;
            }

            bool isTower;
            if (towerManager.CurrentLevel == 1)
            {
                isTower = true;
            }
            else if (availableTowers.Count == 0)
            {
                isTower = false;
            }
            else if (availableConsumables.Count == 0)
            {
                isTower = true;
            }
            else
            {
                isTower = Random.Range(0f, 1f) < towerSpawnPercent;
            }

            if (isTower)
            {
                int randomIndex = Random.Range(0, availableTowers.Count);
                var tower = availableTowers[randomIndex];
                availableTowers.RemoveAt(randomIndex);

                selectTowerUIs[slotIndex].gameObject.SetActive(true);
                selectTowerUIs[slotIndex].SetInteractive(true);
                selectTowerUIs[slotIndex].SetTowerData(tower);
            }
            else
            {
                int randomIndex = Random.Range(0, availableConsumables.Count);
                var consumable = availableConsumables[randomIndex];
                availableConsumables.RemoveAt(randomIndex);

                selectTowerUIs[slotIndex].gameObject.SetActive(true);
                selectTowerUIs[slotIndex].SetInteractive(true);
                selectTowerUIs[slotIndex].SetConsumableData(consumable);
            }
        }

        if(towerManager.CurrentLevel != 1)
        {
            Managers.SoundManager.PlaySFX(AudiosId.ui_menu_popup_message_reward_01);
        }

        Time.timeScale = 0f;
        base.Open();
    }

    public override void Close()
    {
        for (int i = 0; i < selectTowerUIs.Count; i++)
        {
            selectTowerUIs[i].ResetOutline();
        }


        base.Close();
    }


    //Tutorial Methods
    public void TutorialOpen(int towerId)
    {
        for(int i = 0; i < selectTowerUIs.Count; i++)
        {
            selectTowerUIs[i].SetTowerData(towerManager.GetIdToTower(towerId));
        }
        Time.timeScale = 0f;
        base.Open();
    }

    public Button GetSelectButton()
    {
        return selectTowerButton;
    }

    public List<SelectTowerUI> GetSelectTowerUIs()
    {
        return selectTowerUIs;
    }
}
