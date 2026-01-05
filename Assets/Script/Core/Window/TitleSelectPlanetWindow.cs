using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TitleSelectPlanetWindow : Window
{
    [Header("Reference")]
    [SerializeField] private PlanetInfomation planetInfomation;
    [SerializeField] private Transform planetInfomationRoot;

    [Header("Buttons")]
    [SerializeField] private Button closeButton;
    [SerializeField] private Button selectPlanetButton;

    [Header("Description")]
    [SerializeField] private TextMeshProUGUI planetNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [Header("Status")]
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI atkText;
    [SerializeField] private TextMeshProUGUI defText;

    private PlanetTable.Data planetData;
    private PresetData.Data presetData;
    private PlanetInfomation currentSelectInfomation;
    private List<PlanetInfomation> planetInfomations = new List<PlanetInfomation>();
    
    private int presetIndex;
    private WindowIds prevWindowId;

    public override void Close()
    {
        base.Close();
    }

    public override void Init(WindowManager manager)
    {
        base.Init(manager);

        windowId = (int)WindowIds.TitleSelectPlanetWindow;
        closeButton.onClick.AddListener(() => {
            if(prevWindowId != WindowIds.None)
                manager.Open(prevWindowId);
            else
                manager.Open(WindowIds.TitlePresetWindow);
        });
        selectPlanetButton.onClick.AddListener(() => {
            if (planetData == null) return;
            var userData = FirebaseManager.Instance.PlanetData.GetOrigin(planetData.ID);
            if (!userData.UseAble) return;

            var towerPlaceWindow =  manager.Open(WindowIds.TitleTowerPlaceEditWindow);
            
            if(towerPlaceWindow is TitleTowerPlaceEditWindow window)
            {
                window.SetPrevWindow(prevWindowId);
                window.SetPresetData(presetData , presetIndex);
            }
        });

        var planetDatas = DataTableManager.PlanetTable.GetAllData();
        foreach(var data in planetDatas)
        {
            var infomation = Instantiate(planetInfomation, planetInfomationRoot);
            infomation.UpdateTexts(data);
            infomation.OnClickPlanet += GetPlanetData;
            planetInfomations.Add(infomation);
        }
    }

    public override void Open()
    {
        base.Open();
    }

    private void GetPlanetData(PlanetTable.Data planetData , PlanetInfomation selectInfomation)
    {
        if (planetData == null) return;

        currentSelectInfomation?.UpdateOutline(false);

        this.planetData = planetData;
        currentSelectInfomation = selectInfomation;
        currentSelectInfomation?.UpdateOutline(true);

        presetData.PlanetId = planetData.ID;

        Managers.SoundManager.PlaySFX(AudiosId.ui_button_simple_click_07);
        UpdateDescription(planetData);
    }

    private void UpdateDescription(PlanetTable.Data planetData)
    {
        planetNameText.text = planetData.Name;
        descriptionText.text = planetData.Explanation;

        var userData = FirebaseManager.Instance.PlanetData.GetOrigin(planetData.ID);
        var data = DataTableManager.PlanetLevelUpTable.GetData(planetData.ID , userData.level == 0 ? 1 : userData.level);

        hpText.text = data.hp;
        atkText.text = data.atk;
        defText.text = data.def;
    }

    public void SetPresetData(PresetData.Data presetData , int presetIndex)
    {
        this.presetData = presetData;
        this.presetIndex = presetIndex;

        var planetId = presetData.PlanetId;
        var findIdx = planetInfomations.FindIndex(x => x.GetData().ID == planetId);
        var planetData = planetInfomations[findIdx].GetData();

        GetPlanetData(planetData , planetInfomations[findIdx]);
    }

    public void SetPrevWindow(WindowIds prevWindow)
    {
        prevWindowId = prevWindow;
    }
}
