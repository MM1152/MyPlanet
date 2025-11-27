using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfomationTab : MonoBehaviour
{
    [SerializeField] private Sprite starDisAbleSprite;
    [SerializeField] private Sprite starOnEnAbleSprite;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI pieceCountText;
    [SerializeField] private TextMeshProUGUI planetGradeText;
    [SerializeField] private TextMeshProUGUI planetTypeText;
    [SerializeField] private TextMeshProUGUI planetElemetTypeText;
    [SerializeField] private TextMeshProUGUI planetDescriptionText;
    [Header("Images")]
    [SerializeField] private Image planetImage;
    [SerializeField] private Image planetElementTypeImage;
    [SerializeField] private Image[] startsImages;

    public void UpdateData(PlanetTable.Data planetTableData)
    {
        planetGradeText.text = planetTableData.grade;
        planetTypeText.text = planetTableData.PlanetType;
        planetElemetTypeText.text = planetTableData.AttributeType;
        planetDescriptionText.text = planetTableData.Explanation;
        planetElementTypeImage.sprite = DataTableManager.SpriteTable.Get(DataTableIds.ElementSpriteTable, planetTableData.Attribute);

        ResetStar();
        var starCount = FirebaseManager.Instance.PlanetData.GetOrigin(planetTableData.ID).star;
        for(int i = 0; i < starCount; i++)
        {
            startsImages[i].sprite = starOnEnAbleSprite;
        }
    }

    private void ResetStar()    
    {
        for(int i = 0; i < startsImages.Length; i++)
        {
            startsImages[i].sprite = starDisAbleSprite;
        }
    }
}
