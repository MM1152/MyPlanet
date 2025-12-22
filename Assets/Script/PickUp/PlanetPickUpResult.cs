using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlanetPickUpResult : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI gradeText;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI elementText;
    [SerializeField] private TextMeshProUGUI planetNameText;
    [SerializeField] private TextMeshProUGUI newText;
    [SerializeField] private TextMeshProUGUI sliderText;

    [Header("Images")]
    [SerializeField] private Image planetImage;
    [SerializeField] private Image planetTypeImage;
    [SerializeField] private Image elemetImage;

    [Header("Buttons")]
    [SerializeField] private Button okButton;

    [Header("References")]
    [SerializeField] private WindowManager windowManager;

    [Header("Sliders")]
    [SerializeField] private Slider planetPieceSlider;

    public Button OkButton => okButton;
    private void Awake()
    {
        okButton.onClick.AddListener(() => windowManager.Open(WindowIds.RandomPickUpWindow));
    }

    public void SetData(RandomPickUpTable.Data randomPlanetData, bool isNew , (bool, float) isDuplication)
    {
        var planetData = DataTableManager.PlanetTable.Get(randomPlanetData.connection_id);

        if (randomPlanetData.reward_type == 1)
        {
            planetImage.sprite = planetData.PlanetImage;
        }
        else if (randomPlanetData.reward_type == 2)
        {
            planetImage.sprite = randomPlanetData.RewardData.ItemData.ItemImage;
        }

        gradeText.text = planetData.grade;
        typeText.text = planetData.PlanetType;
        planetNameText.text = randomPlanetData.RewardName;
        elemetImage.sprite = DataTableManager.SpriteTable.Get(DataTableIds.ElementSpriteTable, planetData.Attribute);


        var userPlanetData = FirebaseManager.Instance.PlanetData.GetOrigin(planetData.ID);
        var needPeiceCount = userPlanetData.NeedPeiceCount * planetData.NeedPeiceCountPercent;
        if(!userPlanetData.UseAble)
        {
            needPeiceCount = 10;
        }
        sliderText.text = $"조각 개수 {userPlanetData.count} / {needPeiceCount}";
        planetPieceSlider.value = (float)userPlanetData.count / needPeiceCount;

        newText.gameObject.SetActive(isNew);
    }
}
