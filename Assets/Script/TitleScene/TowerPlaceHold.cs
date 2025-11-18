using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerPlaceHold : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI indexText;
    private Outline outline;
    private Image image;

    public Button button;

    public int index;
    public int Index => index;

    private TowerTable.Data towerData = null;
    public TowerTable.Data TowerData => towerData;
    public void Init()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
        button = GetComponent<Button>();
        image = GetComponent<Image>();

        image.color = Color.gray;
    }

    public void UpdateText(int index)
    {
        this.index = index;
        indexText.text = (index + 1).ToString();
    }

    public void PlaceTower(TowerTable.Data tower)
    {
        towerData = tower;
        if(towerData == null)
        {
            image.color = Color.gray;
        }
        else
        {
            image.color = Color.white;
        }
    }

    public bool Placed()
    {
        return towerData != null;
    }

    public void Select()
    {
        outline.enabled = true;
    }

    public void CancelSelect()
    {
        outline.enabled = false;
    }
}   
