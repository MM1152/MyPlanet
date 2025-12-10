using Cysharp.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryWindow : Window
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI playTimeText;

    [Header("Buttons")]
    [SerializeField] private Button replayButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button nextStageButton;

    [SerializeField] private GameObject victoryTitle;
    [SerializeField] private GameObject victoryTextBackground;
    [SerializeField] private GameObject failTitle;
    [SerializeField] private GameObject failTextBackground;
    [SerializeField] private GameObject nextStageButtonObject;

    [SerializeField] private GameObject goldLayout;
    [SerializeField] private GameObject expLayout;
    [SerializeField] private GameObject diamondLayout;

    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI expText;
    [SerializeField] private TextMeshProUGUI diamondText;

    [SerializeField] private WaveManager waveManager;

    private int goldReward;
    private int expReward;
    private int diamondReward;

    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        windowId = (int)WindowIds.VictoryWindow;

        if (Variable.IsTutorialActive)
        {
            replayButton.interactable = false;
        }

        goldLayout.SetActive(false);
        expLayout.SetActive(false);
        diamondLayout.SetActive(false);

        replayButton.onClick.AddListener(() => OnClickReplayButton().Forget());
        exitButton.onClick.AddListener(() => OnClickExitButton().Forget());
    }

    public override void Open()
    {
        Time.timeScale = 0f;
        base.Open();
    }

    public override void Close()
    {
        base.Close();
    }

    private async UniTaskVoid OnClickReplayButton()
    {
        var task = FirebaseManager.Instance.UserData.GetGoods(goldReward, expReward, diamondReward);
        await Managers.Instance.WaitForLoadingAsync(task);

        Time.timeScale = (int)GameSpeed.CurrentSpeed;;
        SceneManager.LoadScene(SceneIds.LoadingScene);
    }

    private async UniTaskVoid OnClickExitButton()
    {
        // Time.timeScale = 1f;
        var task = FirebaseManager.Instance.UserData.GetGoods(goldReward , expReward , diamondReward);
        await Managers.Instance.WaitForLoadingAsync(task);

        manager.Close();
        LoadingScene.sceneId = SceneIds.TitleScene;
        SceneManager.LoadScene(SceneIds.LoadingScene);
    }

    public void SetVictoryUI(float timer, bool isClear,bool lastStage)
    {
        bool isTutorial = Variable.IsTutorialActive;

        if (isClear)
        {
            victoryTitle.SetActive(true);
            victoryTextBackground.SetActive(true);
            failTitle.SetActive(false);
            failTextBackground.SetActive(false);
        }
        else
        {
            victoryTitle.SetActive(false);
            victoryTextBackground.SetActive(false);
            failTitle.SetActive(true);
            failTextBackground.SetActive(true);
        }

        SetRewards(isClear);
        replayButton.interactable = !isTutorial;    
        nextStageButton.interactable = (isClear && !lastStage && !isTutorial);
        playTimeText.text = $"플레이 타임 | {(int)(timer / 60):00}분 {(int)(timer % 60):00}초";
    }

    public void SetRewards(bool isClear)
    {
        var stageId = FirebaseManager.Instance.PresetData.GetGameData().stageId;
        var rewardData = DataTableManager.StageInfomationTable.Get(stageId);
        var maxStage = DataTableManager.WaveTable.GetStageData(stageId).waveGroups.Count;

        var percent = waveManager.CurrentWaveIndex / (float)maxStage;

        if (isClear)
        {
            goldLayout.SetActive(true);
            expLayout.SetActive(true);
            diamondLayout.SetActive(true);
            goldText.text = $"+ {rewardData.CLEAR_REWARD1_COUNT}";
            expText.text = $"+ {rewardData.CLEAR_REWARD2_COUNT}";
            diamondText.text = $"+ {rewardData.CLEAR_REWARD3_COUNT}";

            goldReward = rewardData.CLEAR_REWARD1_COUNT;
            expReward = rewardData.CLEAR_REWARD2_COUNT;
            diamondReward = rewardData.CLEAR_REWARD3_COUNT;
        }
        else
        {
            goldLayout.SetActive(true);
            expLayout.SetActive(true);
            goldText.text = $"+ {(int)(rewardData.CLEAR_REWARD1_COUNT * percent)}";
            expText.text = $"+ {(int)(rewardData.CLEAR_REWARD2_COUNT * percent)}";

            goldReward = (int)(rewardData.CLEAR_REWARD1_COUNT * percent);
            expReward = (int)(rewardData.CLEAR_REWARD2_COUNT * percent);
            diamondReward = 0;
        }
    }
}
