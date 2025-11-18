using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlanetInfomation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI planetNameText;
    [SerializeField] private TextMeshProUGUI planetGradeText;
    [SerializeField] private TextMeshProUGUI planetTypeText;

    private Outline outline;
    private PlanetTable.Data data;
    public event Action<PlanetTable.Data , PlanetInfomation> OnClickPlanet;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    public void UpdateTexts(PlanetTable.Data data)
    {
        this.data = data;
        planetNameText.text = data.Name;
        planetGradeText.text = data.grade;
        planetTypeText.text = data.PlanetType;
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
        if(Managers.TouchManager.TouchType == TouchTypes.Tab && Managers.TouchManager.OnTargetUI(gameObject))
        {
            OnClickPlanet?.Invoke(data, this);
        }
    }
}
