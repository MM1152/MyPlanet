using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyGiftLayout : MonoBehaviour
{
    [SerializeField] private Image giftImage;
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private Button button;
    [SerializeField] private Image checkBox;

    public void SetGiftData(DailyGiftTable.Data data)
    {
        giftImage.sprite = data.ItemData.ItemImage;
        dayText.text = "Day " + data.ID.ToString();
        valueText.text = data.Num.ToString();
    }

    public void SetInteraction(bool active)
    {
        button.interactable = active;
    }

    public void SetCheckBox(bool active)
    {
        checkBox.gameObject.SetActive(active);
    }
}
