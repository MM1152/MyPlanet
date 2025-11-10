using TMPro;
using UnityEngine;

public class SelectTowerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private TowerTable.Data towerData;
    
    public void SetTowerData(TowerTable.Data data)
    {
        this.towerData = data;
        text.text = data.Name.ToString();
    }

    public TowerTable.Data GetTowerData()
    {
        return towerData;
    }
}
