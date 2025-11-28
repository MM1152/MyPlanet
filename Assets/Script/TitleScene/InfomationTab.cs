using Cysharp.Threading.Tasks;
using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfomationTab : MonoBehaviour
{
    [SerializeField] private Sprite starDisAbleSprite;
    [SerializeField] private Sprite starOnEnAbleSprite;

    [Header("Sliders")]
    [SerializeField] private Slider pieceCountSlider;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI pieceCountText;
    [SerializeField] private TextMeshProUGUI planetGradeText;
    [SerializeField] private TextMeshProUGUI planetTypeText;
    [SerializeField] private TextMeshProUGUI planetElemetTypeText;
    [SerializeField] private TextMeshProUGUI planetDescriptionText;
    [Header("Images")]
    [SerializeField] private Image planetImage;
    [SerializeField] private Image planetElementTypeImage;
    [SerializeField] private Image[] startsImages;

#if DEBUG_MODE
    [SerializeField] private Button debugAddPieceButton;
#endif

    private PlanetTable.Data planetTableData;

    private void Awake()
    {
#if DEBUG_MODE
        debugAddPieceButton.gameObject.SetActive(true);
        debugAddPieceButton.onClick.AddListener(() =>
        {
            OnClickAddPiece().Forget();
        });
#endif
    }

    public void UpdateData(PlanetTable.Data planetTableData)
    {
        if (this.planetTableData != null)
            FirebaseManager.Instance.Database.RemoveListner(string.Format(DataBasePaths.PlanetPeiceCountPathFormating, this.planetTableData.ID) , OnValueChangedPieceCount);

        this.planetTableData = planetTableData; 
        FirebaseManager.Instance.Database.AddListner(string.Format(DataBasePaths.PlanetPeiceCountPathFormating, this.planetTableData.ID) , OnValueChangedPieceCount);

        planetGradeText.text = planetTableData.grade;
        planetTypeText.text = planetTableData.PlanetType;
        planetElemetTypeText.text = planetTableData.AttributeType;
        planetDescriptionText.text = planetTableData.Explanation;
        planetElementTypeImage.sprite = DataTableManager.SpriteTable.Get(DataTableIds.ElementSpriteTable, planetTableData.Attribute);

#if DEBUG_MODE
        debugAddPieceButton.interactable = true;
        var userPlanetData = FirebaseManager.Instance.PlanetData.GetOrigin(planetTableData.ID);
        if (!userPlanetData.UseAble)
        {
            debugAddPieceButton.interactable = false;
        }
#endif
            ResetStar();
        var starCount = FirebaseManager.Instance.PlanetData.GetOrigin(planetTableData.ID).star;
        for(int i = 0; i < starCount; i++)
        {
            startsImages[i].sprite = starOnEnAbleSprite;
        }
    }

    private void ResetStar()    
    {
        for(int i = 0; i < startsImages.Length; i++)
        {
            startsImages[i].sprite = starDisAbleSprite;
        }
    }

    private void OnValueChangedPieceCount(object sender , ValueChangedEventArgs args)
    {
        var pieceCount = int.Parse(args.Snapshot.Value.ToString());
        var userPlnaetData = FirebaseManager.Instance.PlanetData.GetOrigin(planetTableData.ID);
        var maxPieceCount = (int)(planetTableData.NeedPeiceCountPercent * userPlnaetData.NeedPeiceCount);
        
        if(maxPieceCount == 0)
            pieceCountText.text = "조각 개수 : MAX";
        else
            pieceCountText.text = $"조각 개수 : {pieceCount}/{maxPieceCount}";

        pieceCountSlider.value = (float)pieceCount / maxPieceCount;
    }
#if DEBUG_MODE
    private async UniTaskVoid OnClickAddPiece()
    {
        debugAddPieceButton.interactable = false;
        await FirebaseManager.Instance.PlanetData.AddPieceCountAsync(planetTableData.ID , 100);
        debugAddPieceButton.interactable = true;
    }
#endif
}
