using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class WaveWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveNumberText;

    [SerializeField] private TextMeshProUGUI waveTimerText;
    [SerializeField] private SliderValue bossHealthSlider;
    [SerializeField] private TextMeshProUGUI bosscurrentHpText;
    [SerializeField] private TextMeshProUGUI bossNameText;
    [SerializeField] private TextMeshProUGUI bossTotalHpText;
    [SerializeField] private Image terraformingHightlightImage;
    [SerializeField] private Image waveBackgroundImage;

    public Image TerraformingHighlightImage => terraformingHightlightImage;
    private void Awake()
    {
        terraformingHightlightImage.gameObject.SetActive(false);
    }

    public void SetWaveText(int waveNumber)
    {
        waveNumberText.text = $"WAVE {waveNumber}";
    }

    public void SetWaveTimerText(float timeRemaining)
    {
        waveTimerText.text = $"{(int)(timeRemaining/60):00}:{(int)(timeRemaining%60):00}";
    }
    
    public  void SetWaveBackgroundImage(Sprite sprite)
    {
       if(waveBackgroundImage != null)
        {
            waveBackgroundImage.sprite = sprite;
        }
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
