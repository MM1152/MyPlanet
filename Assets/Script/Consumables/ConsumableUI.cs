using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableUI : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] private Image consumableImage;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI consumableNameText;
    [SerializeField] private TextMeshProUGUI cosumableDurationText;

    private Consumable consumable;
    public bool IsSetting => consumable != null;

    public void SetConsumable(Consumable cosumable)
    {
        this.consumable = cosumable;
        this.consumable.SetUI(this.gameObject);
        consumableNameText.text = consumable.ConsumData.Name;
    }

    private void Update()
    {
        if (consumable != null)
        {
            consumable.Update(Time.deltaTime);
            cosumableDurationText.text = $"{(int)consumable.GetDuration()}√ ";
            if (consumable.GetDuration() <= 0f)
            {
                consumable = null;
            }
        }
    }

}
