using Firebase.Database;
using JetBrains.Annotations;
using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlanetInfomation : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI planetTypeText;
    [SerializeField] private TextMeshProUGUI planetGradeText;
    [SerializeField] private TextMeshProUGUI planetNameText;
    [SerializeField] private TextMeshProUGUI planetLevelText;
    
    [Header("Image")]
    [SerializeField] private Image planetElemetImage;
    [SerializeField] private Image planetImage;
    [SerializeField] private Image[] starImages;

    [Header("GameObject")]
    [SerializeField] private GameObject disAblePanel;

    [Header("Sprite")]
    [SerializeField] private Sprite disableStarSprite;
    [SerializeField] private Sprite enableStarSprite;

    private Outline outline;
    private PlanetTable.Data data;
    private PlanetData.Data userData;
    public event Action<PlanetTable.Data , PlanetInfomation> OnClickPlanet;

    private bool isSetting = true;
    
    private void Awake()
    {
        outline = GetComponent<Outline>();
        if (outline != null)
            outline.enabled = false;
    }

    private void OnDestroy()
    {
        RemoveFirebaseListeners();
    }

    private void RemoveFirebaseListeners()
    {
        if (data != null && FirebaseManager.Instance != null && FirebaseManager.Instance.Database != null)
        {
            try
            {
                var path = string.Format(DataBasePaths.PlanetStarCountPathFormating, data.ID);
                var levelPath = string.Format(DataBasePaths.PlanetLevelPathFormating, data.ID);
                var unlockPath = string.Format(DataBasePaths.PlanetUnlockPathFormating, data.ID);

                FirebaseManager.Instance.Database.RemoveListner(path, OnValueChangedStar);
                FirebaseManager.Instance.Database.RemoveListner(levelPath, OnValueChangedLevel);
                FirebaseManager.Instance.Database.RemoveListner(unlockPath, OnValueChangedUnLock);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"PlanetInfomation: Failed to remove Firebase listeners: {e.Message}");
            }
        }
    }

    public void UpdateTexts(PlanetTable.Data data)
    {
        // GameObject가 파괴되었는지 확인
        if (this == null || gameObject == null) return;
        
        RemoveFirebaseListeners();
        
        this.data = data;

        if (data == null) return;

        var path = string.Format(DataBasePaths.PlanetStarCountPathFormating, data.ID);
        var levelPath = string.Format(DataBasePaths.PlanetLevelPathFormating, data.ID);
        var unlockPath = string.Format(DataBasePaths.PlanetUnlockPathFormating, data.ID);
        
        FirebaseManager.Instance.Database.AddListner(path, OnValueChangedStar);
        FirebaseManager.Instance.Database.AddListner(levelPath, OnValueChangedLevel);
        FirebaseManager.Instance.Database.AddListner(unlockPath, OnValueChangedUnLock);

        userData = FirebaseManager.Instance.PlanetData.GetOrigin(data.ID);

        if (planetNameText != null) planetNameText.text = data.Name;
        if (planetGradeText != null)
        {
            planetGradeText.text = data.grade;
            planetGradeText.color = data.GradeToColor;
        }
        if (planetElemetImage != null)
            planetElemetImage.sprite = DataTableManager.SpriteTable.Get(DataTableIds.ElementSpriteTable, data.Attribute);
        if (planetTypeText != null) planetTypeText.text = data.PlanetType;
        if (planetLevelText != null) planetLevelText.text = $"Lv. {userData.level:D2}";

        if (planetImage != null) planetImage.sprite = data.PlanetImage;

        ResetStar();
        UpdateStar(userData.star);
        UpdateDisAble();
    }

    private void ResetStar()
    {
        if (starImages == null) return;
        
        for(int i = 0; i < starImages.Length; i++)
        {
            if (starImages[i] != null)
                starImages[i].sprite = disableStarSprite;
        }
    }

    private void UpdateStar(int starCount)
    {
        if (starImages == null) return;
        
        for (int i = 0; i < starCount && i < starImages.Length; i++)
        {
            if (starImages[i] != null)
                starImages[i].sprite = enableStarSprite;
        }
    }

    private void UpdateDisAble()
    {
        // GameObject 파괴 여부 및 컴포넌트 null 체크
        if (this == null || gameObject == null || disAblePanel == null) 
            return;

        if (userData != null && userData.UseAble)
            disAblePanel.SetActive(false);
        else if (disAblePanel != null)
            disAblePanel.SetActive(true);
    }

    private void UpdateLevel(int level)
    {
        if (this == null || gameObject == null || planetLevelText == null) 
            return;
        
        planetLevelText.text = $"Lv. {level:D2}";
    }

    public PlanetTable.Data GetData()
    {
        return data;
    }

    public void UpdateOutline(bool isOn)
    {
        if (outline != null)
        {
            outline.enabled = isOn;
        }
    }

    private void Update()
    {
        if(!Variable.IsTutorialActive && isSetting && Managers.TouchManager.TouchType == TouchTypes.Tab && Managers.TouchManager.OnTargetUI(gameObject))
        {
            OnClickPlanet?.Invoke(data, this);
        }
    }

    private void OnValueChangedStar(object sender , ValueChangedEventArgs args)
    {
        // GameObject 파괴 여부 확인
        if (this == null || gameObject == null) return;
        
        UpdateStar(int.Parse(args.Snapshot.Value.ToString()));
    }

    private void OnValueChangedLevel(object sender, ValueChangedEventArgs args)
    {
        // GameObject 파괴 여부 확인
        if (this == null || gameObject == null) return;
        
        UpdateLevel(int.Parse(args.Snapshot.Value.ToString()));
    }

    private void OnValueChangedUnLock(object sender, ValueChangedEventArgs args)
    {
        // GameObject 파괴 여부 확인 - 주요 에러 발생 지점
        if (this == null || gameObject == null) return;
        
        UpdateDisAble();
    }
}
