using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TerraformingStatusPopUpWindow : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] private Button closeButton;

    public void Awake()
    {
        closeButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
          
        });
    }
    public void SetUI(string name, int level, string description)
    {
        nameText.text = name;
        levelText.text = $"{level} 단계";
        descriptionText.text = description;
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
