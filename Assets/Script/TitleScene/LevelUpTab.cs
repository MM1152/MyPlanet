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

    [Header("Buttons")]
    [SerializeField] private Button levelUpButton;

    private PlanetTable.Data planetData;
    private PlanetData.Data planetUserData;

    private void Awake()
    {
        levelUpButton.onClick.AddListener(() => UpgradePlanet().Forget());

    }

    public void UpdateData(PlanetTable.Data planetData)
    {
        this.planetData = planetData;
        planetUserData = FirebaseManager.Instance.PlanetData.GetOrigin(planetData.ID);
        UpdateText();
    }

    public void UpdateText()
    {
        if (!planetUserData.UseAble) return;
        var upgradeData  = DataTableManager.PlanetLevelUpTable.GetData(planetData.ID , planetUserData.level + 1);
        var prevData  = DataTableManager.PlanetLevelUpTable.GetData(planetData.ID , planetUserData.level);
        if(upgradeData != null)
        {
            hpText.text = prevData.HP + " >> " + upgradeData.HP;
            atkText.text = prevData.ATK + " >> " + upgradeData.ATK;
            defText.text = prevData.DEF + " >> " + upgradeData.DEF;
            goldText.text = upgradeData.Gold.ToString();
            expText.text = upgradeData.Exp.ToString();
        }
        else
        {
            hpText.text = prevData.HP.ToString();
            atkText.text = prevData.ATK.ToString();
            defText.text = prevData.DEF.ToString();
            goldText.text = prevData.Gold.ToString();
            expText.text = prevData.Exp.ToString();
        }
    }

    private async UniTaskVoid UpgradePlanet()
    {
        var goldPath = DataBasePaths.GoldPath;
        var expPath = DataBasePaths.ExpPath;

        var goldResult = await Managers.Instance.WaitForLoadingAsync(FirebaseManager.Instance.Database.GetDataToValue(goldPath));
        var expResult = await Managers.Instance.WaitForLoadingAsync(FirebaseManager.Instance.Database.GetDataToValue(expPath));

        var needGold = DataTableManager.PlanetLevelUpTable.GetData(planetData.ID , planetUserData.level + 1).Gold;
        var needExp = DataTableManager.PlanetLevelUpTable.GetData(planetData.ID , planetUserData.level + 1).Exp;

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
    }
}
