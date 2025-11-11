using TMPro;
using UnityEngine;

public class SelectTowerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private TowerTable.Data towerData;

    public void SetTowerData(Tower data)
    {
        this.towerData = data.TowerData;
        text.text = data.ID.ToString() + "\n";
        text.text += data.Option.GetOptionStringFormatting();
    }

    public TowerTable.Data GetTowerData()
    {
        return towerData;
    }
}
