using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TowerPickUpViewer : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] private Image typeImage;
    [SerializeField] private Image elemeteImage;
    [SerializeField] private Image attackTypeImage;
    [SerializeField] private Image towerImage;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI towerNameText;
    [SerializeField] private TextMeshProUGUI newText;
    [SerializeField] private TextMeshProUGUI sliderText;
    [SerializeField] private TextMeshProUGUI okButtonText;

    [Header("Buttons")]
    [SerializeField] private Button closeButton;
    [SerializeField] private Button skipButton;

    [Header("Slider")]
    [SerializeField] private Slider slider;

    [Header("Ref")]
    [SerializeField] private WindowManager windowManager;
    [SerializeField] private TowerPickUpResult towerPickUpResult;

    private int currentIdx = 0;
    private List<RandomPickUpTable.Data> randomPickUpData;
    private List<bool> isNew;
    private List<(bool, float)> isDuplication;

    private void Awake()
    {
        skipButton.onClick.AddListener(OnClickSkipButton);
    }

    public void SetData(List<RandomPickUpTable.Data> randomPickUpData , List<bool> isNew  , List<(bool, float)> isDuplication)
    {
        closeButton.onClick.RemoveAllListeners();

        this.randomPickUpData = randomPickUpData;
        this.isNew = isNew;
        this.isDuplication = isDuplication;
        this.currentIdx = 0;

        UpdateUI(randomPickUpData[currentIdx], isNew[currentIdx]);

        if (randomPickUpData.Count == 1)
        {
            okButtonText.text = "확인";
            closeButton.onClick.AddListener(OnClickCloseToClose);
        }
        else
        {
            okButtonText.text = $"확인 {currentIdx + 1}/10";
            closeButton.onClick.AddListener(OnClickCloseButtonToNextTowerInfoamtion);
        }
    }

    private void UpdateUI(RandomPickUpTable.Data data , bool isNew)
    {
        var tower = DataTableManager.TowerTable.Get(data.connection_id);
        typeImage.sprite = tower.TypeImage;
        attackTypeImage.sprite = tower.AttackTypeImage;
        elemeteImage.sprite = tower.ElementImage;

        towerNameText.text = tower.Name;
        newText.gameObject.SetActive(isNew);
    }

    private void OnClickCloseToClose()
    {
        windowManager.Open(WindowIds.RandomPickUpWindow);
        closeButton.onClick.RemoveAllListeners();
    }

    private void OnClickCloseButtonToShowPickUpResult()
    {
        towerPickUpResult.gameObject.SetActive(true);
        towerPickUpResult.Setdatas(randomPickUpData , isNew , isDuplication);
        gameObject.SetActive(false);
    }

    private void OnClickCloseButtonToNextTowerInfoamtion()
    {
        currentIdx++;
        if(currentIdx >= randomPickUpData.Count - 1)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(OnClickCloseButtonToShowPickUpResult);
        }

        UpdateUI(randomPickUpData[currentIdx], isNew[currentIdx]);
        okButtonText.text = $"확인({currentIdx + 1}/10)";
    }

    private void OnClickSkipButton()
    {
        OnClickCloseButtonToShowPickUpResult();
    }
}
