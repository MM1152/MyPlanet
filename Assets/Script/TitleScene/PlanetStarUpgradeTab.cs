using Cysharp.Threading.Tasks;
using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

using static UnityEngine.UIElements.UxmlAttributeDescription;

public class PlanetStarUpgradeTab : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Sprite enableStar;
    [SerializeField] private Sprite disableStar;
    [Header("Sliders")]
    [SerializeField] private Slider starCountSlider;
    [Header("Images")]
    [SerializeField] private Image[] starImages;
    [SerializeField] private Image planetImage;
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI currentPeiceCountText;
    [SerializeField] private TextMeshProUGUI unlockSlotText;
    [SerializeField] private TextMeshProUGUI needPeiceCountText;
    [Header("Buttons")]
    [SerializeField] private Button upgradeButton;

    private int needPieceCount;
    private int currentPieceCount;
    private PlanetTable.Data planetTableData;
    private PlanetData.Data userPlanetData;

    private void Awake()
    {
        upgradeButton.onClick.AddListener(() => {
            OnClickUpgradeButton().Forget();
        });    
    }

    public void UpdateData(PlanetTable.Data planetTableData)
    {
        if (this.planetTableData != null)
            FirebaseManager.Instance.Database.RemoveListner(string.Format(DataBasePaths.PlanetPeiceCountPathFormating , this.planetTableData.ID) ,OnValueChangeCount);

        this.planetTableData = planetTableData;
        FirebaseManager.Instance.Database.AddListner(string.Format(DataBasePaths.PlanetPeiceCountPathFormating , this.planetTableData.ID) ,OnValueChangeCount);
        userPlanetData = FirebaseManager.Instance.PlanetData.GetOrigin(planetTableData.ID);
        planetImage.sprite = planetTableData.PlanetImage;
        planetImage.preserveAspect = true;
        upgradeButton.interactable = true;
        UpdateData();
    }

    private void UpdateData()
    {
        ResetStar();
        UpdateStar(userPlanetData.star);
        UpdatePeiceCount(planetTableData, userPlanetData);
        UpdateUnlockSlotText(planetTableData, userPlanetData);
    }

    private void UpdatePeiceCount(PlanetTable.Data planetTableData , PlanetData.Data userPlanetData)
    {
        // 5성까지 찍었을떄
        if(!userPlanetData.unlocked)
        {
            starCountSlider.value = 1f;
            currentPeiceCountText.text = $"해금하기 : {userPlanetData.count}/10";
            needPeiceCountText.text = $"해금하기";
            starCountSlider.value = (float)userPlanetData.count / 10;
            return;
        }

        if (userPlanetData.NeedPeiceCount == 0)
        {
            upgradeButton.interactable = false;
            starCountSlider.value = 1f;
            currentPeiceCountText.text = "MAX";
            needPeiceCountText.text = "MAX";
            return;
        }

        needPieceCount = (int)(planetTableData.NeedPeiceCountPercent * userPlanetData.NeedPeiceCount);
        currentPieceCount = userPlanetData.count;

        currentPeiceCountText.text = $"조각 개수 : {currentPieceCount}/{needPieceCount}";
        starCountSlider.value = (float)currentPieceCount / needPieceCount;

        needPeiceCountText.text = "필요한 조각 개수 : " + needPieceCount.ToString();
    }


    private void ResetStar()
    {
        for(int i = 0; i < starImages.Length; i++)
        {
            starImages[i].sprite = disableStar;
        }
    }

    private void UpdateStar(int starCount)
    {
        for(int i = 0; i < starCount; i++)
        {
            starImages[i].sprite = enableStar;
        }
    }

    private async UniTask OnClickUpgradeButton()
    {
        if (!userPlanetData.unlocked && userPlanetData.count >= 10)
        {
            List<UniTask> tasks = new List<UniTask>();

            tasks.Add(FirebaseManager.Instance.PlanetData.UnlockPlanetAsync(planetTableData.ID));
            tasks.Add(FirebaseManager.Instance.PlanetData.AddPieceCountAsync(planetTableData.ID, -10));

            await Managers.Instance.WaitForLoadingAsync(tasks);
            upgradeButton.interactable = true;
            UpdateData();
            return;
        }

        if (needPieceCount == 0)
            return;


        if (needPieceCount <= currentPieceCount)
        {
            upgradeButton.interactable = false;
            var tasks = FirebaseManager.Instance.PlanetData.UpgradeStarAsync(planetTableData.ID , needPieceCount);
            await Managers.Instance.WaitForLoadingAsync(tasks);
            upgradeButton.interactable = true;
            UpdateData();
        }
    }

    private int GetUnlockSlotCount(string grade , int starLevel)
    {
        // 등급별 기본 ID 계산
        int baseId = grade switch
        {
            "C" => 5041,
            "B" => 5065,
            "A" => 5071,
            "S" => 5077,
            _ => 0
        };

        if (baseId == 0) return 0;

        // 성급에 따른 최종 ID 계산 (0성~5성)
        int finalId = baseId + starLevel;
        
        // OptionTable에서 데이터 가져오기
        return DataTableManager.OptionTable.GetValueDataToInt(finalId);
    }

    private void UpdateUnlockSlotText(PlanetTable.Data planetTableData, PlanetData.Data userPlanetData)
    {
        int currentSlots = DataTableManager.PlanetTable.GetUnlockAbleSlotCount(planetTableData.ID , userPlanetData.star);
        int nextSlots = userPlanetData.star < 5 ? DataTableManager.PlanetTable.GetUnlockAbleSlotCount(planetTableData.ID, userPlanetData.star + 1) : 0;
        
        if(!userPlanetData.unlocked)
        {
            unlockSlotText.text = $"0 >> {currentSlots}";
            return;
        }

        if(nextSlots == 0)
        {
            unlockSlotText.text = $"{currentSlots} (MAX)";
            return;
        }
        else
        {
            unlockSlotText.text = $"{currentSlots} >> {nextSlots}";
        }
    }

    private void OnValueChangeCount(object sender , ValueChangedEventArgs args)
    {
        UpdatePeiceCount(planetTableData, userPlanetData);
    }
}
