using TMPro;
using UnityEngine;

public class PriorityTextLayout : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemPriority;

    public void UpdateTexts(string itemName , string itemPriority)
    {
        this.itemName.color = Color.white;
        this.itemPriority.color = Color.white;

        this.itemName.text = itemName;
        this.itemPriority.text = itemPriority;
    }
}
