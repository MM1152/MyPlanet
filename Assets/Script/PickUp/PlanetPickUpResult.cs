using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlanetPickUpResult : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI gradeText;
    [SerializeField] private TextMeshProUGUI gradeTextToSlider;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI planetNameText;
    [SerializeField] private TextMeshProUGUI newText;
    [SerializeField] private TextMeshProUGUI sliderText;

    [Header("Images")]
    [SerializeField] private Image planetImage;
    [SerializeField] private Image lockImage;
    [SerializeField] private Image elemetImage;

    [Header("Buttons")]
    [SerializeField] private Button okButton;

    [Header("References")]
    [SerializeField] private WindowManager windowManager;

    [Header("Sliders")]
    [SerializeField] private Slider planetPieceSlider;
    private void Awake()
    {
        okButton.onClick.AddListener(() => windowManager.Open(WindowIds.RandomPickUpWindow));
    }

    public void SetData(RandomPickUpTable.Data randomPlanetData, bool isNew , (bool, float) isDuplication)
    {
        var planetData = DataTableManager.PlanetTable.Get(randomPlanetData.connection_id);
        gradeText.text = planetData.grade;
        gradeTextToSlider.text = planetData.grade;
        typeText.text = planetData.PlanetType;
        planetNameText.text = randomPlanetData.RewardName;
        elemetImage.sprite = DataTableManager.SpriteTable.Get(DataTableIds.ElementSpriteTable, planetData.Attribute);

        var userPlanetData = FirebaseManager.Instance.PlanetData.GetOrigin(planetData.ID);

        var needPeiceCount = userPlanetData.NeedPeiceCount * planetData.NeedPeiceCountPercent;
        if(!userPlanetData.UseAble)
        {
            needPeiceCount = 10;
        }
        sliderText.text = $"{userPlanetData.count} / {needPeiceCount}";
        planetPieceSlider.value = (float)userPlanetData.count / needPeiceCount;

        newText.gameObject.SetActive(isNew);
        lockImage.gameObject.SetActive(!userPlanetData.UseAble);
    }
}
