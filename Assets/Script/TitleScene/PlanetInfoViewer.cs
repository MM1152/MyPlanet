using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlanetInfoViewer : MonoBehaviour
{
    [SerializeField] private LevelUpTab levelUpTab;
    [SerializeField] private InfomationTab infomationTab;
    [SerializeField] private PlanetStarUpgradeTab starUpgradeTab;

    [SerializeField] private Image changeColorBackGround;
    [SerializeField] private GameObject infomationTabBackGround;
    [SerializeField] private GameObject levelUpTabBackGround;
    [SerializeField] private GameObject starUpgradeBackGround;
    [Header("Buttons")]
    [SerializeField] private Button infomationButton;
    [SerializeField] private Button levelUpbutton;
    [SerializeField] private Button starUpgradeButton;
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI planetName;
    [SerializeField] private TextMeshProUGUI levelText;

    private GameObject currentTab;
    private GameObject currentBackGround;

    private PlanetTable.Data planetData;
    private PlanetData.Data planetUserData;

    private void Awake()
    {
        infomationButton.onClick.AddListener(() => UpdateTab(infomationTab.gameObject, infomationTabBackGround, infomationButton.image.color));
        levelUpbutton.onClick.AddListener(() => UpdateTab(levelUpTab.gameObject,levelUpTabBackGround, levelUpbutton.image.color));
        starUpgradeButton.onClick.AddListener(() => UpdateTab(starUpgradeTab.gameObject, starUpgradeBackGround, starUpgradeButton.image.color));

        levelUpTab.gameObject.SetActive(false);
        infomationTab.gameObject.SetActive(false);
        starUpgradeTab.gameObject.SetActive(false);

        infomationTabBackGround.gameObject.SetActive(false);
        levelUpTabBackGround.gameObject.SetActive(false);
        starUpgradeBackGround.gameObject.SetActive(false);
    }

    public void UpdatePlanetData(PlanetTable.Data planetData)
    {
        if(this.planetData != null)
            FirebaseManager.Instance.Database.RemoveListner(string.Format(DataBasePaths.PlanetLevelPathFormating, this.planetData.ID), OnValueChangedLevel);

        this.planetData = planetData;
        FirebaseManager.Instance.Database.AddListner(string.Format(DataBasePaths.PlanetLevelPathFormating, planetData.ID), OnValueChangedLevel);

        planetUserData = FirebaseManager.Instance.PlanetData.GetOrigin(planetData.ID);

        planetName.text = this.planetData.Name;
        levelText.text = $"Lv.{planetUserData.level}";
        levelUpTab.UpdateData(planetData);
        infomationTab.UpdateData(planetData);
        starUpgradeTab.UpdateData(planetData);
         
        UpdateTab(infomationTab.gameObject , infomationTabBackGround , infomationButton.image.color);
        CheckUseAblePlanet(planetUserData);
    }

    private void CheckUseAblePlanet(PlanetData.Data data)
    {
        var useAble = data.UseAble;
        if (!useAble)
        {
            levelUpbutton.interactable = false;
        }
        else
        {
            levelUpbutton.interactable = true;
        }
    }

    public void UpdateTab(GameObject tabObject , GameObject backGround , Color color)
    {
        if (currentTab != null)
        {
            currentTab.SetActive(false);
            currentBackGround.SetActive(false);
        }
        currentTab = tabObject;
        currentBackGround = backGround;

        currentTab.SetActive(true);
        currentBackGround.SetActive(true);
    }

    private void OnValueChangedLevel(object sender , ValueChangedEventArgs args)
    {
        levelText.text = string.Format("LV.{0}", args.Snapshot.Value.ToString());
    }

    private void OnValueChangeUnlock(object sender, ValueChangedEventArgs args)
    {
        bool isUnlocked = bool.Parse(args.Snapshot.Value.ToString());
        if (isUnlocked)
        {
            levelUpbutton.interactable = true;
        }
        else
        {
            levelUpbutton.interactable = false;
        }
    }
}
