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
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private TextMeshProUGUI defenceText;

    private PlanetTable.Data planetData;
    private PresetData.Data presetData;
    private PlanetInfomation currentSelectInfomation;
    private List<PlanetInfomation> planetInfomations = new List<PlanetInfomation>();
    
    private int presetIndex;

    public override void Close()
    {
        base.Close();
    }

    public override void Init(WindowManager manager)
    {
        base.Init(manager);

        windowId = (int)WindowIds.TitleSelectPlanetWindow;
        closeButton.onClick.AddListener(() => manager.Open(WindowIds.TitlePresetWindow));
        selectPlanetButton.onClick.AddListener(() => {
            if (planetData == null) return;
            var userData = FirebaseManager.Instance.PlanetData.GetOrigin(planetData.ID);
            if (!userData.UseAble) return;

            var towerPlaceWindow =  manager.Open(WindowIds.TitleTowerPlaceEditWindow);
            if(towerPlaceWindow is TitleTowerPlaceEditWindow window)
            {
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

        UpdateDescription(planetData);
    }

    private void UpdateDescription(PlanetTable.Data planetData)
    {
        planetNameText.text = planetData.Name;
        descriptionText.text = planetData.Explanation;

        hpText.text = planetData.HP.ToString();
        attackText.text = planetData.ATK.ToString();
        defenceText.text = planetData.DEF.ToString();
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

}
