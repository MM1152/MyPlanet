using Firebase.Database;
using JetBrains.Annotations;
using System;
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
        outline.enabled = false;
        
    }

    public void UpdateTexts(PlanetTable.Data data)
    {
        var path = string.Format(DataBasePaths.PlanetStarCountPathFormating, data.ID);
        var levelPath = string.Format(DataBasePaths.PlanetLevelPathFormating, data.ID);
        FirebaseManager.Instance.Database.RemoveListner(path, OnValueChangedStar);
        FirebaseManager.Instance.Database.RemoveListner(levelPath, OnValueChangedLevel);
        this.data = data;

        path = string.Format(DataBasePaths.PlanetStarCountPathFormating, data.ID);
        levelPath = string.Format(DataBasePaths.PlanetLevelPathFormating, data.ID);
        FirebaseManager.Instance.Database.AddListner(path, OnValueChangedStar);
        FirebaseManager.Instance.Database.AddListner(levelPath, OnValueChangedLevel);

        userData = FirebaseManager.Instance.PlanetData.GetOrigin(data.ID);

        planetNameText.text = data.Name;
        planetGradeText.text = data.grade;
        planetGradeText.color = data.GradeToColor;
        planetElemetImage.sprite = DataTableManager.SpriteTable.Get(DataTableIds.ElementSpriteTable, data.Attribute);
        planetTypeText.text = data.PlanetType;
        planetLevelText.text = $"Lv. {userData.level:D2}";
        ResetStar();
        UpdateStar(userData.star);

        if (userData.UseAble)
            disAblePanel.SetActive(false);
        else
            disAblePanel.SetActive(true);
    }

    private void ResetStar()
    {
        for(int i = 0; i < starImages.Length; i++)
        {
            starImages[i].sprite = disableStarSprite;
        }
    }

    private void UpdateStar(int starCount)
    {
        for (int i = 0; i < starCount; i++)
        {
            starImages[i].sprite = enableStarSprite;
        }
    }

    private void UpdateLevel(int level)
    {
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
        if(isSetting && Managers.TouchManager.TouchType == TouchTypes.Tab && Managers.TouchManager.OnTargetUI(gameObject))
        {
            OnClickPlanet?.Invoke(data, this);
        }
    }

    private void OnValueChangedStar(object sender , ValueChangedEventArgs args)
    {
        UpdateStar(int.Parse(args.Snapshot.Value.ToString()));
    }

    private void OnValueChangedLevel(object sender, ValueChangedEventArgs args)
    {
        UpdateLevel(int.Parse(args.Snapshot.Value.ToString()));
    }
}
