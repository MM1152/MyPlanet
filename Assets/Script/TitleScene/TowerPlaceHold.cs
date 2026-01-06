using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.MessageBox;

public class TowerPlaceHold : MonoBehaviour
{
    [SerializeField] private GameObject disAbleSlot;
    [SerializeField] private GameObject unlockSlot;
    [SerializeField] private TowerInfomation towerInfo;
    private Outline outline;
    private Image image;
    [HideInInspector] public Button button;
    [SerializeField] private Image towerImage;

    public int index;
    public int Index => index;

    private bool disAble = false;
    public bool DisAble => disAble;

    private bool unLockAble;
    public bool UnLockAble => unLockAble;

    public Dictionary<int, TowerTable.Data> applyBonusOptionValueTowerTable = new Dictionary<int, TowerTable.Data>();
    public float AttackBonusAmount { get; set; } = 0f;
    public float AttackSpeedBonusAmount { get; set; } = 0f;

    private TowerTable.Data towerData = null;
    public TowerTable.Data TowerData => towerData;

    public void Init()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        disAbleSlot.SetActive(false);
        unlockSlot.SetActive(false);
    }

    public void UpdateSlot(int index)
    {
        // -1 이면 못쓰는 부분
        // 0이면 열린 부분 ( 설치 가능 )
        if (index == -1)
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
    }

    public void PlaceTower(TowerTable.Data tower)
    {
        towerData = tower;
        if (towerData == null)
        {
            towerInfo.gameObject.SetActive(false);
        }
        else
        {
            towerInfo.gameObject.SetActive(true);
            towerInfo.Init(tower.ID);
        }
        CheckElemetTypeToAddBonus();
    }

    public void SetUnLockAble(bool unlockAble)
    {
        this.unLockAble = unlockAble;

        if (unLockAble)
        {
            unlockSlot.SetActive(true);
        }
        else
        {
            unlockSlot.SetActive(false);
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

    public void GetBonusOptionDataTowerIndex(int idx, TowerTable.Data towerData)
    {
        if (applyBonusOptionValueTowerTable.ContainsKey(idx))
        {
            return;
        }

        applyBonusOptionValueTowerTable.Add(idx, towerData);

        if (this.towerData != null)
        {
            CheckElemetTypeToAddBonus();
        }
    }

    public void RemoveBonusOptionDataTowerIndex(int idx)
    {
        if (!applyBonusOptionValueTowerTable.ContainsKey(idx))
        {
            return;
        }
        //optionTable.Add(3, new Data() { id = 3, option = new FireElemetDamageUpgradeOption() });
        //optionTable.Add(4, new Data() { id = 4, option = new IceElemetDamageUpgradeOption() });
        //optionTable.Add(5, new Data() { id = 5, option = new SteelElemetDamageUpgradeOption() });
        //optionTable.Add(6, new Data() { id = 6, option = new LightElemetDamageUpgradeOption() });
        //optionTable.Add(7, new Data() { id = 7, option = new DarkElemetDamageUpgradeOption() });

        //Fire = 1,
        //Ice = 2,
        //Steel = 3,
        //Light = 4,
        //Dark = 5

        applyBonusOptionValueTowerTable.Remove(idx);

        if (this.towerData != null)
        {
            CheckElemetTypeToAddBonus();
        }
    }

    // 아무타워가 안껴져있는 상태면 일단 데이터만 저장
    // 그다음에 타워를 끼게 되면 속성 체크 이후 보너스 적용

    public void CheckElemetTypeToAddBonus()
    {
        AttackBonusAmount = 0f;
        AttackSpeedBonusAmount = 0f;

        foreach (var key in applyBonusOptionValueTowerTable.Keys)
        {
            var towerData = applyBonusOptionValueTowerTable[key];
            var optionData = RandomOptionData.GetData(towerData.Option);

            if (this.towerData != null && optionData.id == this.towerData.attribute + 2)
            {
                AttackBonusAmount += FirebaseManager.Instance.TowerData.Get(towerData.ID).OptionValue;
            }
            else if (optionData.id == 1)
            {
                AttackBonusAmount += FirebaseManager.Instance.TowerData.Get(towerData.ID).OptionValue;
            }
            else if (optionData.id == 2)
            {
                AttackSpeedBonusAmount += FirebaseManager.Instance.TowerData.Get(towerData.ID).OptionValue;
            }
        }
    }

    public void SetActiveTowerImage(bool active)
    {
        if (towerImage == null) return;

        disAbleSlot.SetActive(!active);
        var color = towerImage.color;
        color.a = active ? 1f : 0.4f;
        towerImage.color = color;
    }
}
