using Cysharp.Threading.Tasks;
using Firebase.Database;
using NUnit.Framework;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NUnit.Framework.Interfaces;

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
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI atkText;
    [SerializeField] private TextMeshProUGUI defText;
    [Header("Images")]
    [SerializeField] private Image planetImage;
    [SerializeField] private Image planetElementTypeImage;
    [SerializeField] private Image planetTypeImage;
    [SerializeField] private Image[] startsImages;

#if DEBUG_MODE
    [SerializeField] private Button debugAddPieceButton;
    [SerializeField] private Button unlockButton;
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
        unlockButton.onClick.AddListener(() => OnClickUnlock().Forget());
#endif
    }


    public void UpdateData(PlanetTable.Data planetTableData)
    {
        if (this.planetTableData != null)
        {
            FirebaseManager.Instance.Database.RemoveListner(string.Format(DataBasePaths.PlanetPeiceCountPathFormating, this.planetTableData.ID), OnValueChangedPieceCount);
            FirebaseManager.Instance.Database.RemoveListner(string.Format(DataBasePaths.PlanetStarCountPathFormating, this.planetTableData.ID), OnValueChangedStarCount);
            FirebaseManager.Instance.Database.RemoveListner(string.Format(DataBasePaths.PlanetUnlockPathFormating, this.planetTableData.ID), OnValueChangedUnlock);
            FirebaseManager.Instance.Database.RemoveListner(string.Format(DataBasePaths.PlanetLevelPathFormating, this.planetTableData.ID), OnValueChangedLevel);
        }

        this.planetTableData = planetTableData; 
        FirebaseManager.Instance.Database.AddListner(string.Format(DataBasePaths.PlanetPeiceCountPathFormating, this.planetTableData.ID) , OnValueChangedPieceCount);
        FirebaseManager.Instance.Database.AddListner(string.Format(DataBasePaths.PlanetStarCountPathFormating, this.planetTableData.ID), OnValueChangedStarCount);
        FirebaseManager.Instance.Database.AddListner(string.Format(DataBasePaths.PlanetUnlockPathFormating, this.planetTableData.ID), OnValueChangedUnlock);
        FirebaseManager.Instance.Database.AddListner(string.Format(DataBasePaths.PlanetLevelPathFormating, this.planetTableData.ID), OnValueChangedLevel);


        planetGradeText.text = planetTableData.grade;
        planetGradeText.color = planetTableData.GradeToColor;
        planetTypeText.text = planetTableData.PlanetType;
        planetElemetTypeText.text = planetTableData.AttributeType;
        planetDescriptionText.text = planetTableData.Explanation;
        planetElementTypeImage.sprite = DataTableManager.SpriteTable.Get(DataTableIds.ElementSpriteTable, planetTableData.Attribute);
        planetImage.sprite =  planetTableData.PlanetImage;
        planetImage.preserveAspect = true;
        planetTypeImage.sprite = planetTableData.PlanetTypeImage;
        var userPlanetData = FirebaseManager.Instance.PlanetData.GetOrigin(planetTableData.ID);
        UpdatePeiceCount(userPlanetData.count);
#if DEBUG_MODE
        debugAddPieceButton.interactable = true;
        if (!userPlanetData.UseAble)
        {
            debugAddPieceButton.interactable = false;
        }
        unlockButton.interactable = !userPlanetData.UseAble;
#endif
        ResetStar();
        var starCount = FirebaseManager.Instance.PlanetData.GetOrigin(planetTableData.ID).star;
        for(int i = 0; i < starCount; i++)
        {
            startsImages[i].sprite = starOnEnAbleSprite;
        }

        UpdateText();
    }

    private void ResetStar()    
    {
        for(int i = 0; i < startsImages.Length; i++)
        {
            startsImages[i].sprite = starDisAbleSprite;
        }
    }

    private void UpdatePeiceCount(int pieceCount)
    {
        var userPlnaetData = FirebaseManager.Instance.PlanetData.GetOrigin(planetTableData.ID);
        var maxPieceCount = (int)(planetTableData.NeedPeiceCountPercent * userPlnaetData.NeedPeiceCount);

        if (!userPlnaetData.UseAble) maxPieceCount = 10;

        if (maxPieceCount == 0)
            pieceCountText.text = "조각 개수 : MAX";
        else
            pieceCountText.text = $"조각 개수 : {pieceCount}/{maxPieceCount}";

        pieceCountSlider.value = (float)pieceCount / maxPieceCount;
    }

    private void OnValueChangedPieceCount(object sender , ValueChangedEventArgs args)
    {
        var pieceCount = int.Parse(args.Snapshot.Value.ToString());
        UpdatePeiceCount(pieceCount);
    }

    private void OnValueChangedStarCount(object sender, ValueChangedEventArgs args)
    {
        ResetStar();
        var starCount = int.Parse(args.Snapshot.Value.ToString());
        for (int i = 0; i < starCount; i++)
        {
            startsImages[i].sprite = starOnEnAbleSprite;
        }
    }

    private void OnValueChangedUnlock(object sender, ValueChangedEventArgs args)
    {
        var unlock = bool.Parse(args.Snapshot.Value.ToString());
        unlockButton.interactable = !unlock;
    }

#if DEBUG_MODE
    private async UniTaskVoid OnClickAddPiece()
    {
        debugAddPieceButton.interactable = false;
        await FirebaseManager.Instance.PlanetData.AddPieceCountAsync(planetTableData.ID , 100);
        debugAddPieceButton.interactable = true;
    }

    private async UniTaskVoid OnClickUnlock()
    {
        var userPlnaetData = FirebaseManager.Instance.PlanetData.GetOrigin(planetTableData.ID);
        if (userPlnaetData.count < 10) return;

        var task = new List<UniTask>() {
            FirebaseManager.Instance.PlanetData.UnlockPlanetAsync(planetTableData.ID),
            FirebaseManager.Instance.PlanetData.AddPieceCountAsync(planetTableData.ID , -10)
        };
        await Managers.Instance.WaitForLoadingAsync(task);
    }
#endif
    
    private void UpdateText()
    {
        var planetData = FirebaseManager.Instance.PlanetData.GetOrigin(planetTableData.ID);
        var planetLevelData = DataTableManager.PlanetLevelUpTable.GetData(planetData.id , planetData.level);

        hpText.text = planetLevelData.hp;
        atkText.text = planetLevelData.atk;
        defText.text = planetLevelData.def;
    }

    private void OnValueChangedLevel(object sender, ValueChangedEventArgs args)
    {
        UpdateText();
    }
}
