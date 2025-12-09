using TMPro;
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

    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        windowId = (int)WindowIds.VictoryWindow;

        if (Variable.IsTutorialActive)
        {
            replayButton.interactable = false;
        }
        replayButton.onClick.AddListener(OnClickReplayButton);
        exitButton.onClick.AddListener(OnClickExitButton);
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

    private void OnClickReplayButton()
    {
        Time.timeScale = (int)GameSpeed.CurrentSpeed;;
        SceneManager.LoadScene(SceneIds.LoadingScene);
    }

    private void OnClickExitButton()
    {
        // Time.timeScale = 1f;
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

        replayButton.interactable = !isTutorial;    
        nextStageButton.interactable = (isClear && !lastStage && !isTutorial);
        playTimeText.text = $"플레이 타임 | {(int)(timer / 60):00}분 {(int)(timer % 60):00}초";
        
    }
}
