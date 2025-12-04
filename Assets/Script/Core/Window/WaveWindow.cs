using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class WaveWindow : Window
{
    [SerializeField] private TextMeshProUGUI waveNumberText;

    [SerializeField] private TextMeshProUGUI waveTimerText;

    [SerializeField] private TextMeshProUGUI waveGoldText;

    [SerializeField] private Button backSceneButton;
    [SerializeField] private SliderValue bossHealthSlider;
    [SerializeField] private TextMeshProUGUI bosscurrentHpText;
    [SerializeField] private TextMeshProUGUI bossNameText;
    [SerializeField] private TextMeshProUGUI bossTotalHpText;

    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        windowId = (int)WindowIds.WaveWindow;

        backSceneButton.onClick.AddListener(() => manager.Open(WindowIds.TitleStageSelectedWindow));
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
        waveNumberText.text = $"WAVE {waveNumber}";
    }

    public void SetWaveTimerText(float timeRemaining)
    {
        waveTimerText.text = $"{(int)(timeRemaining/60):00}:{(int)(timeRemaining%60):00}";
    }

    public void ShowBossUI(int maxHP)
    {
        bosscurrentHpText?.gameObject.SetActive(true);
        bossHealthSlider?.gameObject.SetActive(true);
        bossNameText?.gameObject.SetActive(true);
        bossTotalHpText?.gameObject.SetActive(true);
        bosscurrentHpText.text = maxHP.ToString();        
        bossTotalHpText.text = maxHP.ToString();
    }

    public void UpdateBossHP(int currentHP, int maxHP)
    {
        if (bossHealthSlider != null)
        {
            int hp = Mathf.Clamp(currentHP, 0, maxHP);
            bossHealthSlider.UpdateSlider(hp, maxHP, hp);
        }
    }

    public void HideBossUI()
    {
        bosscurrentHpText?.gameObject.SetActive(false);
        bossHealthSlider?.gameObject.SetActive(false);
        bossNameText?.gameObject.SetActive(false);
        bossTotalHpText?.gameObject.SetActive(false);
    }
}
