using Cysharp.Threading.Tasks;
using Firebase.Database;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpTab : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI atkText;
    [SerializeField] private TextMeshProUGUI defText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI expText;

    [Header("Images")]
    [SerializeField] private Image planetImage;
    [SerializeField] private Image[] starImages;

    [Header("Buttons")]
    [SerializeField] private Button levelUpButton;

    [Header("Images")]
    [SerializeField] private Sprite enableStar;
    [SerializeField] private Sprite disableStar;

    private PlanetTable.Data planetData;
    private PlanetData.Data planetUserData;
    [SerializeField] private PopupManager popupManager;

    private void Awake()
    {
        levelUpButton.onClick.AddListener(() => UpgradePlanet().Forget());

    }

    public void UpdateData(PlanetTable.Data planetData)
    {
        FirebaseManager.Instance.Database.RemoveListner(string.Format(DataBasePaths.PlanetStarCountPathFormating, planetData.ID), OnValueChangedStarCount);
        this.planetData = planetData;
        FirebaseManager.Instance.Database.AddListner(string.Format(DataBasePaths.PlanetStarCountPathFormating, planetData.ID), OnValueChangedStarCount);

        levelUpButton.interactable = true;
        planetUserData = FirebaseManager.Instance.PlanetData.GetOrigin(planetData.ID);
        planetImage.sprite = planetData.PlanetImage;
        planetImage.preserveAspect = true;

        UpdateStar(planetUserData.star);
        UpdateText();
    }

    public void UpdateText()
    {
        if (!planetUserData.UseAble) return;
        var upgradeData = DataTableManager.PlanetLevelUpTable.GetData(planetData.ID, planetUserData.level + 1);
        var prevData = DataTableManager.PlanetLevelUpTable.GetData(planetData.ID, planetUserData.level);
        if (upgradeData != null)
        {
            hpText.text = prevData.HP.ToString("N0") + " >> " + upgradeData.HP.ToString("N0");
            atkText.text = prevData.ATK.ToString("N0") + " >> " + upgradeData.ATK.ToString("N0");
            defText.text = prevData.DEF.ToString("N0") + " >> " + upgradeData.DEF.ToString("N0");
            goldText.text = upgradeData.Gold.ToString("N0");
            expText.text = upgradeData.Exp.ToString("N0");
        }
        else
        {
            hpText.text = prevData.HP.ToString("N0");
            atkText.text = prevData.ATK.ToString("N0");
            defText.text = prevData.DEF.ToString("N0");
            goldText.text = "MAX";
            expText.text = "MAX";
            levelUpButton.interactable = false;
        }
    }

    private async UniTaskVoid UpgradePlanet()
    {
        var goldPath = DataBasePaths.GoldPath;
        var expPath = DataBasePaths.ExpPath;

        var goldResult = await Managers.Instance.WaitForLoadingAsync(FirebaseManager.Instance.Database.GetDataToValue(goldPath));
        var expResult = await Managers.Instance.WaitForLoadingAsync(FirebaseManager.Instance.Database.GetDataToValue(expPath));

        var needGold = DataTableManager.PlanetLevelUpTable.GetData(planetData.ID, planetUserData.level + 1).Gold;
        var needExp = DataTableManager.PlanetLevelUpTable.GetData(planetData.ID, planetUserData.level + 1).Exp;

        var gold = int.Parse(goldResult.Item1.ToString());
        var exp = int.Parse(expResult.Item1.ToString());

        if (needExp <= exp && needGold <= gold)
        {
            List<UniTask> tasks = new List<UniTask>() {
                FirebaseManager.Instance.PlanetData.LevelUpPlanetAsync(planetData.ID),
                FirebaseManager.Instance.UserData.UseGoods(needGold , needExp)
            };

            await Managers.Instance.WaitForLoadingAsync(tasks);

            UpdateText();
        }
        else
        {
            var popup = popupManager?.Open<TextPopup>(PopupIds.TextPopup);
            if(popup != null)
            {
                popup.SetTexts("레벨업 실패!", " 재료가 부족합니다. ", "취소", "확인");
                popup.SetButtonAction(() => popupManager.ForceClose(), () => popupManager.ForceClose());
            }
        }
    }

    private void ResetStar()
    {
        for (int i = 0; i < starImages.Length; i++)
        {
            starImages[i].sprite = disableStar;
        }
    }

    private void UpdateStar(int starCount)
    {
        ResetStar();

        for (int i = 0; i < starCount; i++)
        {
            starImages[i].sprite = enableStar;
        }
    }

    private void OnValueChangedStarCount(object sender, ValueChangedEventArgs args)
    {
        var starCount = int.Parse(args.Snapshot.Value.ToString());
        UpdateStar(starCount);
    }
}
