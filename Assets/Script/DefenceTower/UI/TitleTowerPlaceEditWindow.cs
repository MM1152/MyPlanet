using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Runtime.CompilerServices;
using UnityEditor;
using System.Linq;
using Unity.VisualScripting;
using System.Threading.Tasks;

public class TitleTowerPlaceEditWindow : Window
{
    [SerializeField] private TowerInfomation towerInfomation;
    [SerializeField] private Transform towerInfomationRoot;
    [SerializeField] private Image circle;
    [SerializeField] private TowerPlaceHold towerPlaceObject;
    [SerializeField] private ShowIndexPanel showIndexPanel;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private PopupManager popupManager;

    private Vector2 circleSize;
    private int placeCount;

    private List<TowerPlaceHold> placeHolds = new List<TowerPlaceHold>();
    private Dictionary<int,ShowIndexPanel> showIndexPanels = new Dictionary<int, ShowIndexPanel>();
    private List<TowerInfomation> towerInfos = new List<TowerInfomation>();

    private float angle;
    private int selectIndex = 0;
    private bool isRotate = false;

    private TowerFactory towerFactory = new TowerFactory();
    private PresetData.Data presetData;
    private PlanetData.Data planetData;
    private int presetIndex;
    private (int left, int right) prevApplyOptionSlots = (-1, -1);

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
        await FirebaseManager.Instance.PresetData.Save(presetData, presetIndex);
        saveButton.interactable = true;
        manager.Open(WindowIds.TitlePresetWindow);
    }

    public override void Open()
    {
        selectIndex = 0;
        circle.transform.eulerAngles = Vector3.zero;
        isRotate = false;
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
            towerInfo.OnTab += Place;
            towerInfo.OnLongTab += ShowInfomationTower;

            var showIndexPanel = Instantiate(this.showIndexPanel, towerInfo.transform);
            showIndexPanel.OnTab += UnPlace;

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

    private void UnPlace(int idx)
    {
        if (isRotate) return;
        if (!popupManager.Interactable) return;
        showIndexPanels[placeHolds[idx].TowerData.ID].UpdatePlace(-1);
        placeHolds[idx].PlaceTower(null);
        FindOptionApplyTower(null);
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
    }

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
        angle = 360f / placeCount;
        float startAngle = 90f;
        for (int i = 0; i < placeCount; i++)
        {
            var placeHold = Instantiate(towerPlaceObject, circle.transform);

            placeHold.Init();
            placeHold.UpdateText(i);
            placeHold.UpdateSlot(planetData.openSlot[i]);

            RectTransform rect = placeHold.GetComponent<RectTransform>();

            rect.anchoredPosition = new Vector3
            (
                Mathf.Cos(Mathf.Deg2Rad * startAngle),
                Mathf.Sin(Mathf.Deg2Rad * startAngle)
            ) * circleSize * 0.4f;

            startAngle += angle;
            rect.eulerAngles = new Vector3(0, 0, angle * i + 1);
            placeHolds.Add(placeHold);

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
                RotateCircle(idx);
                FindOptionApplyTower(placeHolds[idx].TowerData);
            });
        }

        CheckPlaceHoldUnlockAble();
    }

    private void CheckPlaceHoldUnlockAble()
    {
        var unlockAbleSlotCount = DataTableManager.PlanetTable.GetUnlockAbleSlotCount(planetData.id, planetData.star);

        for(int i = 0; i < unlockAbleSlotCount; i++)
        {
            if (planetData.openSlot[i] == -1)
            {
                placeHolds[i].SetUnLockAble(true);
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
        RotateAsync(currentAngle , rotateAngle , 0.2f).Forget();
        isRotate = true;
        selectIndex = idx;

        placeHolds[selectIndex].transform.localScale = Vector3.one * 1.5f;
        placeHolds[selectIndex].Select();
    }

    private async UniTaskVoid RotateAsync(float from , float to , float duration)
    {
        float delta = Mathf.DeltaAngle(to, from);
        float speed = delta / duration;
        for (float i = 0; i <= duration; i += Time.deltaTime)
        {
            circle.transform.eulerAngles += new Vector3(0f, 0f, speed * Time.deltaTime);
            await UniTask.Yield();
        }

        circle.transform.eulerAngles = new Vector3(0f, 0f, -to);
        isRotate = false;
    }

    private void FindOptionApplyTower(TowerTable.Data towerData) 
    {
        if (prevApplyOptionSlots.left != -1)
        {
            placeHolds[prevApplyOptionSlots.left].CancelSelect();
        }
        if (prevApplyOptionSlots.right != -1)
        {
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
        }
        if (prevApplyOptionSlots.right != -1)
        {
            placeHolds[prevApplyOptionSlots.right].Select();
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
            var popup = popupManager.Open<UnLockPopup>(PopupIds.UnLockPopup);
            popup.Setting(selectIdx , UnLock);
        }
    }

    private void UnLock(int idx)
    {
        var task = FirebaseManager.Instance.PlanetData.UnLockSlotAsync(planetData.id, idx);
        Managers.Instance.WaitForLoadingAsync(task).Forget();
        placeHolds[idx].SetUnLockAble(false);
        placeHolds[idx].UpdateSlot(0);
    }
}
