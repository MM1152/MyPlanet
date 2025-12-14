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

    [Header("Slider")]
    [SerializeField] private Slider partCountSlider;

    [Header("Ref")]
    [SerializeField] private GameObject optionValueBackGround;
    [SerializeField] private GameObject rewardPartBackGround;
    [SerializeField] private GameObject sliderBackGround;

    public void SetData(RandomPickUpTable.Data data , bool isNew , (bool, float) isDuplication)
    {
        maxOrNewText.gameObject.SetActive(false);
        sliderBackGround.gameObject.SetActive(false);
        rewardPartBackGround.SetActive(false);
        optionValueBackGround.gameObject.SetActive(false);

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

        sliderBackGround.gameObject.SetActive(true);
        optionValueBackGround.SetActive(true);
        optionValueText.text = userData.OptionValue + "%";
    }
    // 옵션이 높을때
    private void GetOptionUpgradeTower(TowerData.Data userData , (bool, float) isDuplication) 
    {
        optionValueText.text = $"{isDuplication.Item2}% >> {userData.OptionValue}%";
        optionValueBackGround.SetActive(true);

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
        rewardPartBackGround.SetActive(true);
        sliderBackGround.SetActive(true);

        towerRewardPeiceCountText.text = $"x{((int)isDuplication.Item2).ToString()}";
    }
}
