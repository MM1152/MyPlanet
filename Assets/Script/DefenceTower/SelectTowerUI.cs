using TMPro;
using UnityEngine;

public class SelectTowerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private TowerData.Data towerData;
    
    public void SetTowerData(TowerData.Data data)
    {
        this.towerData = data;
        text.text = data.name;
    }

    public TowerData.Data GetTowerData()
    {
        return towerData;
    }
}
