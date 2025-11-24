using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerPlaceHold : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI indexText;
    [SerializeField] private GameObject disAbleSlot;
    [SerializeField] private Image towerImage;
    [SerializeField] private Image towerImageBackGround;
    private Outline outline;
    private Image image;
    [HideInInspector] public Button button;

    public int index;
    public int Index => index;

    private bool disAble = false;
    public bool DisAble => disAble;

    private TowerTable.Data towerData = null;
    public TowerTable.Data TowerData => towerData;
    public void Init()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        disAbleSlot.SetActive(false);
    }

    public void UpdateSlot(int index)
    {
        // -1 이면 못쓰는 부분
        // 0이면 열린 부분 ( 설치 가능 )
        if( index == -1 )
        {
            disAble = true;
            disAbleSlot.SetActive(true);
        }
        else
        {
            disAble = false;
            disAbleSlot.SetActive(false);
        }
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
            towerImageBackGround.gameObject.SetActive(false);
        }
        else
        {
            towerImage.gameObject.SetActive(true);
            towerImageBackGround.gameObject.SetActive(true);
        }
        image.color = Color.white;
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
