using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlanetInfomation : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI planetTypeText;
    [SerializeField] private TextMeshProUGUI planetGradeText;
    [SerializeField] private TextMeshProUGUI planetLevelText;
    [SerializeField] private TextMeshProUGUI planetNameText;

    [Header("Image")]
    [SerializeField] private Image planetElemetImage;
    [SerializeField] private Image[] starImages;

    private Outline outline;
    private PlanetTable.Data data;
    public event Action<PlanetTable.Data , PlanetInfomation> OnClickPlanet;

    private bool isSetting = true;
    
    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    public void UpdateTexts(PlanetTable.Data data)
    {
        if (data.ID == 1011 || data.ID == 1012)
            isSetting = false;

        this.data = data;
        planetNameText.text = data.Name;
        planetGradeText.text = data.grade;
        planetElemetImage.sprite = DataTableManager.SpriteTable.Get(DataTableIds.ElementSpriteTable, data.Attribute);
        planetTypeText.text = data.AttributeType;
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
}
