using Firebase.Database;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TowerInfomation : MonoBehaviour
{
    [SerializeField] private Sprite enableStar;
    [SerializeField] private Sprite disableStar;

    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image outlineImage;

    [SerializeField] private Image typeImage;
    [SerializeField] private Image effectiveImage;
    [SerializeField] private Image towerAttackType;
    [SerializeField] private Image towerImage;

    [SerializeField] private Image[] starImage;

    [SerializeField] private GameObject typeImageBackGround;
    [SerializeField] private GameObject effectiveImageBackGround;
    [SerializeField] private GameObject towerAttackImageBackGround;

    [SerializeField] private TextMeshProUGUI towerNameText;

    private TowerTable.Data data;
    private TowerData.Data userData;

    public event Action<TowerTable.Data> OnTab;
    public event Action<TowerTable.Data> OnLongTab;
    public bool DisableTouch { get; set; } = false;

    private bool isPressed = false;
    public void Init(int towerId)
    {
        data = DataTableManager.TowerTable.Get(towerId);
        userData = FirebaseManager.Instance.TowerData.Get(towerId);

        towerNameText.text = data.Name; 

        var typeSprite = DataTableManager.SpriteTable.Get(DataTableIds.TypeSpriteTable, data.Type);
        if(typeSprite != null)
            typeImage.sprite = typeSprite;
        else
            typeImageBackGround.SetActive(false);

        var effectiveSprite = DataTableManager.SpriteTable.Get(DataTableIds.ElementSpriteTable, data.attribute);
        if (effectiveSprite != null)
            effectiveImage.sprite = effectiveSprite; 
        else 
            effectiveImageBackGround.SetActive(false);

        var attackTypeSprite = data.AttackTypeImage;
        if (attackTypeSprite != null)
            towerAttackType.sprite = attackTypeSprite;
        else
            towerAttackImageBackGround.SetActive(false);

        FirebaseManager.Instance.Database.AddListner(string.Format(DataBasePaths.TowerGradeFormating, data.ID), OnChangeGrade);

        towerImage.sprite = DataTableManager.TowerTable.Get(towerId).towerImage;
        backgroundImage.color = data.AttributeToColor.backGroundColor;
        outlineImage.color = data.AttributeToColor.outlineColor;

        UpdateStar(userData.grade);
    }

    public TowerTable.Data GetTowerData()
    {
        return data;
    }

    private void Update()
    {
        if(!Variable.IsTutorialActive && !DisableTouch && Managers.TouchManager.TouchType == TouchTypes.Tab && Managers.TouchManager.OnTargetUI(this.gameObject))
        {
            OnTab?.Invoke(data);
            Managers.SoundManager.PlaySFX(AudiosId.ui_menu_button_scroll_page_03);
        }

        if (!isPressed && Managers.TouchManager.TouchType == TouchTypes.LongPress && Managers.TouchManager.OnTargetUI(this.gameObject))
        {
            isPressed = true;
            OnLongTab?.Invoke(data);    
        }

        if (Managers.TouchManager.TouchType == TouchTypes.None)
            isPressed = false;
    }

    private void OnChangeGrade(object sender, ValueChangedEventArgs args)
    {
        var grade = int.Parse(args.Snapshot.Value.ToString());
        UpdateStar(grade);
    }

    public void OnUnlockValueChanged(object sender, ValueChangedEventArgs args)
    {
        var result = bool.Parse(args.Snapshot.Value.ToString());
        if (result)
        {
            gameObject.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        OnTab = null;
        OnLongTab = null;
        FirebaseManager.Instance.Database.RemoveListner(string.Format(DataBasePaths.TowerUnlockPathFormating, data.ID),OnUnlockValueChanged);
        FirebaseManager.Instance.Database.RemoveListner(string.Format(DataBasePaths.TowerGradeFormating, data.ID), OnChangeGrade);
    }

    private void ResetStar()
    {
        for(int i = 0; i < starImage.Length; i++)
        {
            starImage[i].sprite = disableStar;
        }
    }

    private void UpdateStar(int starCount)
    {
        ResetStar();

        for (int i = 0; i < starCount; i++)
        {
            starImage[i].sprite = enableStar;
        }
    }
}
