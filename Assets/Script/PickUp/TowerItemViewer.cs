using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerItemViewer : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI towerNameText;
    [SerializeField] private TextMeshProUGUI optionValueText;
    [SerializeField] private TextMeshProUGUI sliderText;
    [SerializeField] private TextMeshProUGUI maxOrNewText;
    [SerializeField] private TextMeshProUGUI towerRewardPeiceCountText;

    [Header("Image")]
    [SerializeField] private Image towerImage;
    [SerializeField] private Image towerPieceImage;
    [SerializeField] private Image towerPieceImage2;

    [Header("Slider")]
    [SerializeField] private Slider partCountSlider;

    [Header("Ref")]
    [SerializeField] private GameObject bonusTopGameObject;
    [SerializeField] private GameObject[] sliderLayout;
    [SerializeField] private GameObject optionValueLayout;

    public void SetData(RandomPickUpTable.Data data , bool isNew , (bool, float) isDuplication)
    {
        maxOrNewText.gameObject.SetActive(false);

        //towerPieceImage.sprite = DataTableManager.TowerDuplicationRewardTable.GetData(data.connection_id, 1).ItemData.ItemImage;
        //towerPieceImage2.sprite = DataTableManager.TowerDuplicationRewardTable.GetData(data.connection_id, 1).ItemData.ItemImage;
        towerImage.sprite = DataTableManager.TowerTable.Get(data.connection_id).towerImage;
        bonusTopGameObject.SetActive(false);
        sliderLayout[0].SetActive(false);
        sliderLayout[1].SetActive(false);
        optionValueLayout.SetActive(false);

        var userData = FirebaseManager.Instance.TowerData.Get(data.connection_id);

        towerNameText.text = data.RewardName;
        if (isNew)
        {
            GetNewTower(userData);
        }
        else if(isDuplication.Item1)
        {
            GetOptionUpgradeTower(userData , isDuplication);
        }
        else if(!isDuplication.Item1 && isDuplication.Item2 != 0)
        {
            GetOptionDownTower(userData, isDuplication);
        }
    }

    // 새로운 타워 획득시
    private void GetNewTower(TowerData.Data userData)
    {
        maxOrNewText.text = "NEW";
        maxOrNewText.gameObject.SetActive(true);
        maxOrNewText.color = Color.yellow;

        sliderLayout[0].SetActive(true);
        sliderLayout[1].SetActive(true);
        optionValueLayout.SetActive(true);

        optionValueText.text = userData.OptionValue + "%";
    }
    // 옵션이 높을때
    private void GetOptionUpgradeTower(TowerData.Data userData , (bool, float) isDuplication) 
    {
        optionValueLayout.SetActive(true);
        optionValueText.text = $"{isDuplication.Item2}% >> {userData.OptionValue}%";

        var isMax = DataTableManager.TowerRandomOptionValueTable.IsMaxGrade(userData.TowerId, userData.grade, userData.OptionValue);
        if (isMax)
        {
            maxOrNewText.text = "MAX";
            maxOrNewText.gameObject.SetActive(true);
            maxOrNewText.color = Color.red;
        }
    }
    // 옵션이 낮을떄
    private void GetOptionDownTower(TowerData.Data userData , (bool , float) isDuplication)
    {
        bonusTopGameObject.SetActive(true);
        sliderLayout[0].SetActive(true);
        sliderLayout[1].SetActive(true);
        towerRewardPeiceCountText.text = $"x{((int)isDuplication.Item2)}";
    }
}
