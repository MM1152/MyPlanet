using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class WaveWindow : Window
{
    [SerializeField] private TextMeshProUGUI waveNumberText;

    [SerializeField] private TextMeshProUGUI waveTimerText;

    [SerializeField] private TextMeshProUGUI waveGoldText;

    [SerializeField] private Button backSceenButton;

    [SerializeField] private Button statesButton;

    private void Start()
    {
        //버튼 이벤트 셋팅 연결
    }

    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        windowId = (int)WindowIds.WaveWindow;
    }

    public override void Open()
    {
        base.Open();
    }

    public override void Close()
    {
        base.Close();
    }

    public void SetWaveText(int waveNumber)
    {
        waveNumberText.text = $"Wave {waveNumber}";
    }

    public void SetWaveTimerText(float timeRemaining)
    {
        waveTimerText.text = $"{timeRemaining:00}";
    }

    private void BackToMainScreen()
    {
        //메인화면으로 돌아가는 기능
    }

    private void OpenStatesWindow()
    {
        //상태창 오픈 기능
    }   
}
