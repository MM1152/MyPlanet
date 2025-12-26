using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TenPickUpItemLayout : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemGrade;
    [SerializeField] private TextMeshProUGUI sliderText;
    [SerializeField] private TextMeshProUGUI rewardText;
    [Header("Images")]
    [SerializeField] private Image itemImage;
    [SerializeField] private Image rewardImage;
    [SerializeField] private Image plaentImage;

    [Header("References")]
    [SerializeField] private Transform rewardRoot;
    [SerializeField] private GameObject newTextRoot;

    [Header("Sliders")]
    [SerializeField] private Slider slider;
    public void UpdateData(RandomPickUpTable.Data data , bool isNew, (bool ,float) isDuplication)
    {
        var planetData = DataTableManager.PlanetTable.Get(data.connection_id);
        var userData = FirebaseManager.Instance.PlanetData.GetOrigin(data.connection_id);

        itemName.text = data.RewardName;
        itemGrade.text = data.rarityToString;
        itemGrade.color = planetData.GradeToColor;
        newTextRoot.gameObject.SetActive(isNew);
        if(data.RewardData.ItemData.ItemImage != null)
        {
            plaentImage.sprite = data.RewardData.ItemData.ItemImage;
            rewardImage.sprite = data.RewardData.ItemData.ItemImage;
        }
        else
        {
            plaentImage.sprite = DataTableManager.PlanetTable.Get(data.connection_id).PlanetImage;
        }

        rewardRoot.gameObject.SetActive(isDuplication.Item1);
        rewardText.text = $"x{(int)isDuplication.Item2}";

        slider.gameObject.SetActive(!isNew);
        var fullPeiceCount = planetData.NeedPeiceCountPercent * userData.NeedPeiceCount;
        sliderText.text = $"{userData.count} / {fullPeiceCount}";
        slider.value = userData.count / (float)fullPeiceCount;
    }
}
