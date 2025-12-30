using Cysharp.Threading.Tasks.Triggers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TerraformingUI : MonoBehaviour
{
    [SerializeField] private GameObject stateTerraforming;
    [SerializeField] private Button stateOpenButton;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI laverText;
    [SerializeField] private TerraformingStatusPopUpWindow terraformingPopupWindow;
    [SerializeField] private Image terraformingImage;
    public void SetUI(TerraformingTable.Data data)
    {
        stateTerraforming.SetActive(true);
        nameText.text = TerraformingData.GetTerraformingNameDataKey(data.Terra_name);
        laverText.text = $"{data.unlock_point} 단계";
        terraformingImage.sprite = data.T_image;

        stateOpenButton.onClick.RemoveAllListeners();
        stateOpenButton.onClick.AddListener(() =>
        {
            terraformingPopupWindow.SetUI(nameText.text, data.unlock_point, TerraformingData.GetTerraformingDescriptionDataKey(data.T_description), data.T_image);
            terraformingPopupWindow.Open();
        });

    }
}
