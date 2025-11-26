using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlanetInfoViewer : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI planetNameText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI atkText;
    [SerializeField] private TextMeshProUGUI defText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI expText;

    [Header("Images")]
    [SerializeField] private Image planetImage;

    private PlanetTable.Data planetData;

    public void UpdatePlanetData(PlanetTable.Data planetData)
    {
        this.planetData = planetData;
    }

    public void UpdateTexts()
    {
        planetNameText.text = $"LV.00 {planetData.Name}";
        hpText.text = $"데이터 연결 필요";
        atkText.text = $"데이터 연결 필요";
        defText.text = $"데이터 연결 필요";
        goldText.text = $"데이터 연결 필요";
        expText.text = $"데이터 연결 필요";
    }
}
