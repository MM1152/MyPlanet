using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlanetInfoViewer : MonoBehaviour
{
    [SerializeField] private LevelUpTab levelUpTab;
    [SerializeField] private InfomationTab infomationTab;
    [SerializeField] private Image changeColorBackGround;
    [Header("Buttons")]
    [SerializeField] private Button infomationButton;
    [SerializeField] private Button levelUpbutton;
    [SerializeField] private Button starUpgradeButton;
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI planetName;
    private GameObject currentTab;

    private PlanetTable.Data planetData;
    private PlanetData.Data planetUserData;
    private void Awake()
    {
        infomationButton.onClick.AddListener(() => UpdateTab(infomationTab.gameObject, infomationButton.image.color));
        levelUpbutton.onClick.AddListener(() => UpdateTab(levelUpTab.gameObject, levelUpbutton.image.color));

        levelUpTab.gameObject.SetActive(false);
        infomationTab.gameObject.SetActive(false);

    }

    public void UpdatePlanetData(PlanetTable.Data planetData)
    {
        if(this.planetData != null)
            FirebaseManager.Instance.Database.RemoveListner(string.Format(DataBasePaths.PlanetLevelPathFormating, this.planetData.ID), OnValueChangedLevel);

        this.planetData = planetData;
        FirebaseManager.Instance.Database.AddListner(string.Format(DataBasePaths.PlanetLevelPathFormating, planetData.ID), OnValueChangedLevel);

        planetUserData = FirebaseManager.Instance.PlanetData.GetOrigin(planetData.ID);

        planetName.text = $"Lv.{planetUserData.level}."+this.planetData.Name;
        levelUpTab.UpdateData(planetData);
        infomationTab.UpdateData(planetData);
      
        UpdateTab(infomationTab.gameObject , infomationButton.image.color);
        CheckUseAblePlanet(planetUserData);
    }

    private void CheckUseAblePlanet(PlanetData.Data data)
    {
        var useAble = data.UseAble;
        if (!useAble)
        {
            levelUpbutton.interactable = false;
            starUpgradeButton.interactable = false;
        }
        else
        {
            levelUpbutton.interactable = true;
            starUpgradeButton.interactable = true;
        }
    }

    public void UpdateTab(GameObject tabObject , Color color)
    {
        if (currentTab != null)
        {
            currentTab.SetActive(false);
        }
        currentTab = tabObject;
        changeColorBackGround.color = color;
        currentTab.SetActive(true);
    }

    private void OnValueChangedLevel(object sender , ValueChangedEventArgs args)
    {
        planetName.text = string.Format("LV.{0} {1}", args.Snapshot.Value.ToString() , planetData.Name);
    }
}
