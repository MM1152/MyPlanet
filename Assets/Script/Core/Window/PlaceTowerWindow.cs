using System.Collections.Generic;
using System.Linq;
using TMPro;
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

            var inGameWindow = manager.GetWindow(WindowIds.InGamePlaceTowerWindow) as InGamePlaceTowerWindow;
            if (inGameWindow != null)
            {
                inGameWindow.UpdateTowerActivationState();
            }

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

        levelText.text = $"Lv. {towerManager.CurrentLevel}";

        var allTowers = towerManager.GetAllTower().Where(x => x != null).ToList();
        Utils.Suffle(allTowers);

        // 레벨이 1일때는 타입 1 타워만 나오도록 설정
        if (towerManager.CurrentLevel == 1)
        {
            int uiIdx = 0;
            for(int i = 0; i < allTowers.Count && uiIdx < 3; i++)
            {
                if (allTowers[i] == null || allTowers[i].TowerData.Type != 1)
                    continue;

                var nextLevelData = DataTableManager.LevelUpTable.Get(allTowers[i].TowerData.ID, allTowers[i].Level + 1);
                if(nextLevelData != null)
                {
                    selectTowerUIs[uiIdx++].SetTowerData(allTowers[i]);
                }
            }
        }
        // 이후부터는 모든 타워 혹은 소모품이 나올 수 있도록 구현
        else
        {
            var consumes = consumableManager.GetAllData();
            Utils.Suffle(consumes);

            int uiIdx = 0;
            while(uiIdx < 3)
            {
                bool isTower = Random.Range(0f, 1f) < towerSpawnPercent;
                if (isTower)
                {
                    LevelUpTable.Data levelUpData = null;
                    int towerIdx = 0;
                    do
                    {
                        if( towerIdx >= allTowers.Count) break;

                        levelUpData = DataTableManager.LevelUpTable.Get(allTowers[towerIdx].TowerData.ID, allTowers[towerIdx].Level + 1);
                        if(levelUpData != null)
                        {
                            selectTowerUIs[uiIdx++].SetTowerData(allTowers[towerIdx]);
                            allTowers.RemoveAt(towerIdx);
                        }
                        else
                        {
                            towerIdx++;
                        }
            
                    } while (levelUpData == null && towerIdx < allTowers.Count);

                    if(levelUpData == null)
                    {
                        selectTowerUIs[uiIdx++].SetConsumableData(consumes[0]);
                        consumes.RemoveAt(0);
                    }
                }
                else
                {
                    selectTowerUIs[uiIdx++].SetConsumableData(consumes[0]);
                    consumes.RemoveAt(0);
                }
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

        for(int i = 0; i < selectTowerUIs.Count; i++)
        {
            selectTowerUIs[i].gameObject.SetActive(false);
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
