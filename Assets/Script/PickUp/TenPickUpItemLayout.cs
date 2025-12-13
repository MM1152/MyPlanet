using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TenPickUpItemLayout : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemGrade;
    [SerializeField] private TextMeshProUGUI newText;
    [SerializeField] private TextMeshProUGUI sliderText;
    [SerializeField] private TextMeshProUGUI rewardText;
    [Header("Images")]
    [SerializeField] private Image itemImage;
    [SerializeField] private Image rewardImage;

    [Header("References")]
    [SerializeField] private Transform rewardRoot;

    [Header("Sliders")]
    [SerializeField] private Slider slider;
    public void UpdateData(RandomPickUpTable.Data data , bool isNew, bool isDuplication)
    {
        itemName.text = data.RewardName;
        itemGrade.text = data.rarityToString;
        newText.gameObject.SetActive(isNew);
        
        var planetData = DataTableManager.PlanetTable.Get(data.connection_id);
        var userData = FirebaseManager.Instance.PlanetData.GetOrigin(data.connection_id);

        rewardRoot.gameObject.SetActive(isDuplication);
        rewardText.text = "x10";

        slider.gameObject.SetActive(!isNew);
        var fullPeiceCount = planetData.NeedPeiceCountPercent * userData.NeedPeiceCount;
        sliderText.text = $"{userData.count} / {fullPeiceCount}";
        slider.value = userData.count / (float)fullPeiceCount;
    }
}
