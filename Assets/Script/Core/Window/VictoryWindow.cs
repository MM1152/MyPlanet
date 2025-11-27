using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryWindow : Window
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI playTimeText;
    [SerializeField] private TextMeshProUGUI victoryText;

    [Header("Buttons")]
    [SerializeField] private Button replayButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button nextStageButton;

    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        windowId = (int)WindowIds.VictoryWindow;

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

    public void UpdateText(float timer, bool isClear)
    {
        if(isClear)
        {
            victoryText.text = "Victory!";
            playTimeText.text = string.Format("플레이 타임 {0:F2}", timer);
        }
        else
        {
            victoryText.text = "Fail!";
            playTimeText.text = string.Format("플레이 타임 {0:F2}", timer);
        }
    }
}
