using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

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

    [SerializeField] private TextMeshProUGUI towerTypeText;
    [SerializeField] private TextMeshProUGUI towerElementText;
    [SerializeField] private TextMeshProUGUI towerAttackTypeText;

    [SerializeField] private TextMeshProUGUI buffText;

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

    public Button OkButton => closeButton;

    public event Action TutorialTabAction;

    private void Awake()
    {
        skipButton.onClick.AddListener(OnClickSkipButton);
    }

    public void SetData(List<RandomPickUpTable.Data> randomPickUpData , List<bool> isNew  , List<(bool, float)> isDuplication)
    {
        //closeButton.onClick.RemoveAllListeners();

        this.randomPickUpData = randomPickUpData;
        this.isNew = isNew;
        this.isDuplication = isDuplication;
        this.currentIdx = 0;

        UpdateUI(randomPickUpData[currentIdx], isNew[currentIdx], isDuplication[currentIdx]);

        if (randomPickUpData.Count == 1)
        {
            skipButton.gameObject.SetActive(false);
            okButtonText.text = "확인";
            closeButton.onClick.AddListener(OnClickCloseToClose);
        }
        else
        {
            skipButton.gameObject.SetActive(true);
            okButtonText.text = $"확인 {currentIdx + 1}/10";
            closeButton.onClick.AddListener(OnClickCloseButtonToNextTowerInfoamtion);
        }
    }

    private void UpdateUI(RandomPickUpTable.Data data , bool isNew, (bool ,float) isDuplication)
    {
        var tower = DataTableManager.TowerTable.Get(data.connection_id);

        towerImage.sprite = tower.towerImage;

        typeImage.sprite = tower.TypeImage;
        attackTypeImage.sprite = tower.AttackTypeImage;
        if(attackTypeImage.sprite == null)
            attackTypeImage.gameObject.SetActive(false);
        else 
            attackTypeImage.gameObject.SetActive(true);

        elemeteImage.sprite = tower.ElementImage;
        if(elemeteImage.sprite == null)
            elemeteImage.gameObject.SetActive(false);
        else 
            elemeteImage.gameObject.SetActive(true);

        if(isDuplication.Item1)
        {
            buffText.text = $"{isDuplication.Item2}% >> {tower.OptionValue}%";
        }
        else
        {
            buffText.text = $"{tower.OptionValue}%";
        }

        towerTypeText.text = tower.TypeToString;
        towerElementText.text = tower.AttributeToString;
        towerAttackTypeText.text = tower.AttackTypeToString;

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

        UpdateUI(randomPickUpData[currentIdx], isNew[currentIdx] , isDuplication[currentIdx]);
        okButtonText.text = $"확인({currentIdx + 1}/10)";
    }

    private void OnClickSkipButton()
    {
        OnClickCloseButtonToShowPickUpResult();
    }
}
