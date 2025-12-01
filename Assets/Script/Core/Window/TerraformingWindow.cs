using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TerraformingWindow : Window
{
    public Button leftButton;

    public Button rightButton;

    [SerializeField] private TextMeshProUGUI levelText;
    // [SerializeField] private Image leftIconImage;   
    // [SerializeField] private Image rightIconImage;      
    [SerializeField] private TextMeshProUGUI leftTitleText;
    [SerializeField] private TextMeshProUGUI rightTitleText;

    [SerializeField] private TextMeshProUGUI leftDescText;
    [SerializeField] private TextMeshProUGUI rightDescText;

    public override void Close()
    {
        base.Close();
        Time.timeScale = 1f;
    }

    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        windowId = (int)WindowIds.TerraformingWindow;
    }

    public void SetUI(string leftTitle, string leftDesc, string rightTitle, string rightDesc, int point)
    {
        // leftIconImage.sprite = leftIcon;
        leftTitleText.text = leftTitle;
        leftDescText.text = leftDesc;

        // rightIconImage.sprite = rightIcon;
        rightTitleText.text = rightTitle;
        rightDescText.text = rightDesc;

        levelText.text = $"{point} /{TerraformingData.terrformingOpenValues.Length}";
    }

    public override void Open()
    {
        base.Open();      
    }
}

