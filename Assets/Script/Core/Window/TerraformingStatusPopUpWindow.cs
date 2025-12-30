using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TerraformingStatusPopUpWindow : MonoBehaviour
{
    [SerializeField] private Image terraformingImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button closeButton;

    public void Awake()
    {
        closeButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
          
        });
    }
    public void SetUI(string name, int level, string description, Sprite image)
    {
        nameText.text = name;
        levelText.text = $"{level} 단계";
        descriptionText.text = description;
        terraformingImage.sprite = image;
    }
    public void Open()
    {
        gameObject.SetActive(true);
    }   
    public void Close()
    {
        gameObject.SetActive(false);
    }   
}
