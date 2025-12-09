using Firebase.Database;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerInfomation : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image outlineImage;

    [SerializeField] private Image typeImage;
    [SerializeField] private Image effectiveImage;
    [SerializeField] private Image towerAttackType;
    [SerializeField] private TextMeshProUGUI towerNameText;
    private TowerTable.Data data;

    public event Action<TowerTable.Data> OnTab;
    public event Action<TowerTable.Data> OnLongTab;
    public bool DisableTouch { get; set; } = false;

    private bool isPressed = false;
    public void Init(int towerId)
    {
        data = DataTableManager.TowerTable.Get(towerId);
        towerNameText.text = data.Name; 
        typeImage.sprite = DataTableManager.SpriteTable.Get(DataTableIds.TypeSpriteTable , data.Type);
        effectiveImage.sprite = DataTableManager.SpriteTable.Get(DataTableIds.ElementSpriteTable , data.Attribute);

        backgroundImage.color = data.AttributeToColor.backGroundColor;
        outlineImage.color = data.AttributeToColor.outlineColor;
    }

    public TowerTable.Data GetTowerData()
    {
        return data;
    }

    private void Update()
    {
        if(!DisableTouch && Managers.TouchManager.TouchType == TouchTypes.Tab && Managers.TouchManager.OnTargetUI(this.gameObject))
        {
            OnTab?.Invoke(data);
        }

        if (!isPressed && Managers.TouchManager.TouchType == TouchTypes.LongPress && Managers.TouchManager.OnTargetUI(this.gameObject))
        {
            isPressed = true;
            OnLongTab?.Invoke(data);    
        }

        if (Managers.TouchManager.TouchType == TouchTypes.None)
            isPressed = false;
    }

    public void OnUnlockValueChanged(object sender, ValueChangedEventArgs args)
    {
        var result = bool.Parse(args.Snapshot.Value.ToString());
        if (result)
        {
            gameObject.SetActive(true);
        }
    }
}
