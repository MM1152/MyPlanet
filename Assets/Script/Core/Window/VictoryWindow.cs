using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryWindow : Window
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI playTimeText;
    // [SerializeField] private TextMeshProUGUI victoryText;

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

        if(Variable.IsTutorialActive)
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
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneIds.LoadingScene);
    }

    private void OnClickExitButton()
    {
        Time.timeScale = 1f;
        LoadingScene.sceneId = SceneIds.TitleScene;
        SceneManager.LoadScene(SceneIds.LoadingScene);
    }

    public void SetVictoryUI(bool isClear)
    {
        if(isClear)
        {
            victoryTitle.SetActive(true);
            victoryTextBackground.SetActive(true);
            failTitle.SetActive(false);
            failTextBackground.SetActive(false);
            nextStageButtonObject.SetActive(true);
        }
        else
        {
            victoryTitle.SetActive(false);
            victoryTextBackground.SetActive(false);
            failTitle.SetActive(true);
            failTextBackground.SetActive(true);
            nextStageButtonObject.SetActive(false);    
        }
    }

    public void UpdateText(float timer, bool isClear)
    {
        if(isClear)
        {
            // victoryText.text = "Victory!";
            playTimeText.text = string.Format("플레이 타임 | {0:F2}", timer);
        }
        else
        {
            // victoryText.text = "Fail!";
            playTimeText.text = string.Format("플레이 타임 | {0:F2}", timer);
        }
    }
}
