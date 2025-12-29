using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TitleTowerPlaceEditWindow : Window
{
    [SerializeField] private TowerInfomation towerInfomation;
    [SerializeField] private Transform towerInfomationRoot;
    [SerializeField] protected Image circle;
    [SerializeField] private TowerPlaceHold towerPlaceObject;
    [SerializeField] private ShowIndexPanel showIndexPanel;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private PopupManager popupManager;
    [SerializeField] private GameObject linear;
    [SerializeField] private UpgradeLayout upgradeLayout;

    [Header("Images")]
    [SerializeField] private Image firstImage;
    [SerializeField] private Image secondImage;
    [SerializeField] private Sprite[] numbers;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI status1Title;
    [SerializeField] private TextMeshProUGUI status2Title;
    [SerializeField] private TextMeshProUGUI status3Title;
    [SerializeField] private TextMeshProUGUI status1Value;
    [SerializeField] private TextMeshProUGUI status2Value;
    [SerializeField] private TextMeshProUGUI status3Value;

    public Vector2 circleSize;
    private int placeCount;

    private List<TowerPlaceHold> placeHolds = new List<TowerPlaceHold>();
    private List<UpgradeLayout> upgradeLayouts = new List<UpgradeLayout>();
    private Dictionary<int,ShowIndexPanel> showIndexPanels = new Dictionary<int, ShowIndexPanel>();
    private List<TowerInfomation> towerInfos = new List<TowerInfomation>();

    private float angle;
    private int selectIndex = 0;
    private bool isRotate = false;

    private TowerFactory towerFactory = new TowerFactory();
    private PresetData.Data presetData;
    private PlanetData.Data planetData;
    private int presetIndex;
    private WindowIds prevWindow;
    private (int left, int right) prevApplyOptionSlots = (-1, -1);

    protected bool inGameViewer = false;

    public override void Close()
    {
        if(presetData != null)
        {
            presetData.TowerId = placeHolds.Select(x => x.TowerData != null ? x.TowerData.ID : -1).ToList();
        }
        base.Close();
        Release();
    }

    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        windowId = (int)WindowIds.TitleTowerPlaceEditWindow;    

        Canvas.ForceUpdateCanvases();
        circleSize = new Vector3(circle.rectTransform.rect.width , circle.rectTransform.rect.height);

        closeButton.onClick.AddListener(() => manager.Open(WindowIds.TitleSelectPlanetWindow));
        saveButton.onClick.AddListener(() =>
        {
            saveButton.interactable = false;
            WaitForSaveAsync().Forget();
        });
    }

    private async UniTaskVoid WaitForSaveAsync()
    {
        presetData.TowerId = placeHolds.Select(x => x.TowerData != null ? x.TowerData.ID : -1).ToList();
        var task = FirebaseManager.Instance.PresetData.Save(presetData, presetIndex);
        await Managers.Instance.WaitForLoadingAsync(task);
        saveButton.interactable = true;
        if (prevWindow != WindowIds.None)
            manager.Open(prevWindow);
        else
            manager.Open(WindowIds.TitlePresetWindow);
    }

    public override void Open()
    {
        selectIndex = 0;
        circle.transform.eulerAngles = Vector3.zero;
        isRotate = false;
        firstImage.sprite = numbers[(1) % 10];
        secondImage.sprite = numbers[(1) / 10];
        base.Open();
    }

    public void SetPresetData(PresetData.Data presetData , int presetIndex)
    {
        this.presetData = presetData;
        this.presetIndex = presetIndex;
        this.planetData = FirebaseManager.Instance.PlanetData.GetOrigin(this.presetData.PlanetId);
        placeCount = presetData.TowerId.Count;

        UpdateTowerHold();
        UpdateTowerList();
        RotateCircle(0);
    }

    private void Release()
    {
        for (int i = 0; i < placeHolds.Count; i++)
        {
            Destroy(placeHolds[i].gameObject);
        }
        placeHolds.Clear();

        for (int i = 0; i < towerInfos.Count; i++)
        {
            Destroy(towerInfos[i].gameObject);
        }
        towerInfos.Clear();
        showIndexPanels.Clear();
    }

    private void UpdateTowerList()
    {
        var towerList = DataTableManager.TowerTable.GetAll();
        
        for (int i = 0; i < towerList.Count; i++)
        {
            // 일단 임시로 막아놓은거임
            if (!towerFactory.ContainTower(towerList[i].ID))
                continue;

            var towerInfo = Instantiate(towerInfomation, towerInfomationRoot);
            if(!inGameViewer)
            {
                towerInfo.OnTab += Place;
                towerInfo.OnLongTab += ShowInfomationTower;
            }

            var path = string.Format(DataBasePaths.TowerUnlockPathFormating, towerList[i].ID);
            FirebaseManager.Instance.Database.AddListner(path, towerInfo.OnUnlockValueChanged);

            var showIndexPanel = Instantiate(this.showIndexPanel, towerInfo.transform);
            if (!inGameViewer)
            {
                showIndexPanel.OnTab += UnPlace;
            }
            towerInfo.Init(towerList[i].ID);
            towerInfos.Add(towerInfo);

            int curIdx = ContainPresetList(towerList[i].ID);
            if (curIdx != -1 && planetData.openSlot[curIdx - 1] == -1)
            {
                curIdx = -1;
            }
            showIndexPanel.Init(towerInfo);
            showIndexPanel.UpdatePlace(curIdx);

            showIndexPanels.Add(towerList[i].ID, showIndexPanel);

            if (!FirebaseManager.Instance.TowerData.Get(towerList[i].ID).Unlock)
                towerInfo.gameObject.SetActive(false);
        }
    }

    private void ShowInfomationTower(TowerTable.Data towerData)
    {
        var popup = popupManager.Open<TowerInfomationPopup>(PopupIds.TowerInfomationPopup);
        if(popup != null)
        {
            popup.UpdateTexts(towerData);
        }
    }

    private void SwapTower(int idx , TowerTable.Data data)
    {
        UnPlace(selectIndex);
        Place(data);
    }

    private void UnPlace(int idx)
    {
        if (isRotate) return;
        if (!placeHolds[idx].Placed()) return;
        if (!popupManager.Interactable) return;
        
        var towerData = placeHolds[idx].TowerData;
        int targetIndex = towerData.Option_Range;

        if (towerData.Option_type == 0) prevApplyOptionSlots = GetBothSideSlots(idx, targetIndex);
        else if (towerData.Option_type == 1) prevApplyOptionSlots = (GetLeftSlots(idx, targetIndex), -1);
        else if (towerData.Option_type == 2) prevApplyOptionSlots = (-1, GetRightSlots(idx, targetIndex));

        if (prevApplyOptionSlots.left != -1)
            placeHolds[prevApplyOptionSlots.left].RemoveBonusOptionDataTowerIndex(idx);

        if (prevApplyOptionSlots.right != -1)
            placeHolds[prevApplyOptionSlots.right].RemoveBonusOptionDataTowerIndex(idx);


        showIndexPanels[placeHolds[idx].TowerData.ID].UpdatePlace(-1);
        placeHolds[idx].PlaceTower(null);
        FindOptionApplyTower(null);
        Managers.SoundManager.PlaySFX(AudiosId.ui_menu_button_scroll_page_03);
        ResetUpgradeLayout();
        UpdateUpgradeLayout();
        UpdateStatTexts();
    }

    private void Place(TowerTable.Data data)
    {
        if (isRotate) return;
        if (!popupManager.Interactable) return;
        if (!placeHolds[selectIndex].Placed())
        {
            placeHolds[selectIndex].PlaceTower(data);
            showIndexPanels[data.ID].UpdatePlace(selectIndex + 1);
            FindOptionApplyTower(data);            
        }
        else
        {
            SwapTower(selectIndex, data);
            showIndexPanels[data.ID].UpdatePlace(selectIndex + 1);
            FindOptionApplyTower(data);                        
        }
        Managers.SoundManager.PlaySFX(AudiosId.ui_menu_button_scroll_page_03);
        UpdateStatTexts();
    }
    //6.6 , 283.3
    //-11.79999 , 283.3
    private int ContainPresetList(int towerId)
    {
        for(int i = 0; i < presetData.TowerId.Count; i++)
        {
            if(presetData.TowerId[i] == towerId)
            {
                return i + 1;
            }
        }
        return -1;
    }

    private void UpdateTowerHold()
    {
        Canvas.ForceUpdateCanvases();

        angle = 360f / placeCount;
        float startAngle = 90f;
        for (int i = 0; i < placeCount; i++)
        {
            var linear = Instantiate(this.linear, circle.transform);
            var linearRect = linear.GetComponent<RectTransform>();
            linearRect.anchoredPosition = new Vector3
            (
                Mathf.Cos(Mathf.Deg2Rad * startAngle),
                Mathf.Sin(Mathf.Deg2Rad * startAngle)
            ) * circleSize * 0.5f;
            linearRect.eulerAngles = new Vector3(0, 0, angle * i + 1 + 90f);

            var placeHold = Instantiate(towerPlaceObject, circle.transform);

            placeHold.Init();
            placeHold.UpdateText(i);
            placeHold.UpdateSlot(planetData.openSlot[i]);

            RectTransform rect = placeHold.GetComponent<RectTransform>();
            //rect.anchoredPosition = circle.rectTransform.position;
            rect.anchoredPosition = new Vector2
            (
                Mathf.Cos(Mathf.Deg2Rad * startAngle),
                Mathf.Sin(Mathf.Deg2Rad * startAngle)
            ) * circleSize * 0.7f;

            rect.eulerAngles = new Vector3(0, 0, angle * i + 1);
            placeHolds.Add(placeHold);

            var upgradeLayout = Instantiate(this.upgradeLayout, circle.transform);
            upgradeLayout.ResetImages();
            upgradeLayouts.Add(upgradeLayout);
            RectTransform upgradeLayoutRect = upgradeLayout.GetComponent<RectTransform>();
            upgradeLayoutRect.anchoredPosition = new Vector2
            (
                Mathf.Cos(Mathf.Deg2Rad * startAngle),
                Mathf.Sin(Mathf.Deg2Rad * startAngle)
            ) * circleSize * 0.4f;

            upgradeLayoutRect.eulerAngles = new Vector3(0, 0, angle * i + 1);

            startAngle += angle;

            var towerData = DataTableManager.TowerTable.Get(presetData.TowerId[i]);
            if (planetData.openSlot[i] == -1)
            {
                towerData = null;
            }
            placeHold.PlaceTower(towerData);

            int idx = i;
            placeHold.button.onClick.AddListener(() => {
                OpenUnLockSlotPopup(idx);

                if (placeHold.DisAble) return;
                if (idx == selectIndex && !inGameViewer)
                {
                    UnPlace(idx);
                    return;
                }

                RotateCircle(idx);
                firstImage.sprite = numbers[(idx + 1) % 10];
                secondImage.sprite = numbers[(idx + 1) /10];
            });
        }

        CheckPlaceHoldUnlockAble();
    }

    private void CheckPlaceHoldUnlockAble()
    {
        if (inGameViewer) return;

        var unlockAbleSlotCount = DataTableManager.PlanetTable.GetUnlockAbleSlotCount(planetData.id, planetData.star);
        var currentOpenSlotCount = planetData.openSlot.Count(x => x != -1);

        bool unlockAble = unlockAbleSlotCount - currentOpenSlotCount == 0 ? false : true; 

        for(int i = 0; i < placeHolds.Count; i++)
        {
            if(planetData.openSlot[i] == -1)
            {
                placeHolds[i].SetUnLockAble(unlockAble);
            }
        }
    }

    private void RotateCircle(int idx)
    {
        if (isRotate) return;

        placeHolds[selectIndex].transform.localScale = Vector3.one;
        placeHolds[selectIndex].CancelSelect();

        float rotateAngle = angle * idx;
        float currentAngle = angle * selectIndex;
        isRotate = true;
        RotateAsync(currentAngle , rotateAngle , 0.2f).Forget();
        selectIndex = idx;

        placeHolds[selectIndex].transform.localScale = Vector3.one * 1.5f;
        placeHolds[selectIndex].Select();
        Managers.SoundManager.PlaySFX(AudiosId.ui_menu_button_scroll_05);
        ResetUpgradeLayout();
        UpdateStatTexts();
        FindOptionApplyTower(placeHolds[idx].TowerData);
        UpdateUpgradeLayout();
    }

    private async UniTaskVoid RotateAsync(float from , float to , float duration)
    {
        float delta = Mathf.DeltaAngle(to, from);
        float speed = delta / duration;
        for (float i = 0; i <= duration; i += Time.unscaledDeltaTime)
        {
            circle.transform.eulerAngles += new Vector3(0f, 0f, speed * Time.unscaledDeltaTime);
            await UniTask.Yield();
        }

        circle.transform.eulerAngles = new Vector3(0f, 0f, -to);
        isRotate = false;
    }

    private void FindOptionApplyTower(TowerTable.Data towerData) 
    {
        if (prevApplyOptionSlots.left != -1)
        {
            upgradeLayouts[prevApplyOptionSlots.left].ResetGiveUpgrade();
            placeHolds[prevApplyOptionSlots.left].CancelSelect();
        }
        if (prevApplyOptionSlots.right != -1)
        {
            upgradeLayouts[prevApplyOptionSlots.right].ResetGiveUpgrade();
            placeHolds[prevApplyOptionSlots.right].CancelSelect();
        }

        prevApplyOptionSlots = (-1, -1);
        if (towerData == null) return;
        int targetIndex = towerData.Option_Range;

        if (towerData.Option_type == 0) prevApplyOptionSlots = GetBothSideSlots(selectIndex, targetIndex); 
        else if (towerData.Option_type == 1) prevApplyOptionSlots = (GetLeftSlots(selectIndex, targetIndex) , -1); 
        else if (towerData.Option_type == 2) prevApplyOptionSlots = (-1 , GetRightSlots(selectIndex, targetIndex));

        if (prevApplyOptionSlots.left != -1)
        {
            placeHolds[prevApplyOptionSlots.left].Select();
            placeHolds[prevApplyOptionSlots.left].GetBonusOptionDataTowerIndex(selectIndex , placeHolds[selectIndex].TowerData);
            upgradeLayouts[prevApplyOptionSlots.left].GiveUpgrade();
        }
        if (prevApplyOptionSlots.right != -1)
        {
            placeHolds[prevApplyOptionSlots.right].Select();
            placeHolds[prevApplyOptionSlots.right].GetBonusOptionDataTowerIndex(selectIndex , placeHolds[selectIndex].TowerData);
            upgradeLayouts[prevApplyOptionSlots.right].GiveUpgrade();
        }

        placeHolds[selectIndex].Select();
    }

    private int GetLeftSlots(int index , int targetIndex )
    {
        return Utils.ClampIndex(index - targetIndex , placeHolds.Count);
    }

    private int GetRightSlots(int index , int targetIndex)
    {
        return Utils.ClampIndex(index + targetIndex, placeHolds.Count);
    }

    private (int left , int right) GetBothSideSlots(int index , int targetIndex)
    {
        int left = GetLeftSlots(index , targetIndex);
        int right = GetRightSlots(index , targetIndex);

        return (left, right);
    }

    private void OpenUnLockSlotPopup(int selectIdx)
    {
        if (placeHolds[selectIdx].UnLockAble)
        {
            var popup = popupManager?.Open<UnLockPopup>(PopupIds.UnLockPopup);
            if(popup != null)
            {
                popup.Setting(selectIdx, UnLock);
            }
        }
    }

    private void UnLock(int idx)
    {
        UnLockAsync(idx).Forget();
    }

    private async UniTaskVoid UnLockAsync(int idx)
    {
        var task = FirebaseManager.Instance.PlanetData.UnLockSlotAsync(planetData.id, idx);
        await Managers.Instance.WaitForLoadingAsync(task);
        CheckPlaceHoldUnlockAble();
        placeHolds[idx].UpdateSlot(0);
        placeHolds[idx].SetUnLockAble(false);
    }

    public void SetPrevWindow(WindowIds windowId)
    {
        prevWindow = windowId;
    }

    private void UpdateStatTexts()
    {
        TowerTable.Data currentData = placeHolds[selectIndex].TowerData;

        if(currentData == null)
        {
            status1Title.text = "-";
            status1Value.text = "-";
            status2Title.text = "-";
            status2Value.text = "-";
            status3Title.text = "-";
            status3Value.text = "-";
            return;
        }

         
        status1Title.text = currentData.GetFormaingStat1.Split(' ')[0];
        status1Value.text = currentData.GetFormaingStat1.Split(' ')[1];
        status2Title.text = currentData.GetFormaingStat2.Split(' ')[0];
        status2Value.text = currentData.GetFormaingStat2.Split(' ')[1];
        status3Title.text = currentData.GetFormaingStat3.Split(' ')[0];
        status3Value.text = currentData.GetFormaingStat3.Split(' ')[1];

        if (currentData.Type == 2) return;

        status1Value.text += $"<color=#ffbf00> +{placeHolds[selectIndex].AttackBonusAmount.ToString("F2")}% </color>";
        status2Value.text += $"<color=#ffbf00> +{placeHolds[selectIndex].AttackSpeedBonusAmount.ToString("F2")}% </color>";
    }

    private void UpdateUpgradeLayout()
    {
        foreach(var otherTowerIndex in placeHolds[selectIndex].applyBonusOptionValueTowerTable.Keys)
        {
            upgradeLayouts[otherTowerIndex].ResiveUpgrade();
        }
    }

    private void ResetUpgradeLayout()
    {
        foreach (var upgradeLayout in upgradeLayouts)
        {
            upgradeLayout.ResetImages();
        }
    }
}
