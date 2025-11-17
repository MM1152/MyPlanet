using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Runtime.CompilerServices;
using UnityEditor;
using System.Linq;

public class TitleTowerPlaceEditWindow : Window
{
    [SerializeField] private TowerInfomation towerInfomation;
    [SerializeField] private Transform towerInfomationRoot;
    [SerializeField] private Image circle;
    [SerializeField] private TowerPlaceHold towerPlaceObject;
    [SerializeField] private ShowIndexPanel showIndexPanel;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button closeButton;

    private Vector2 circleSize;
    private int placeCount = 10;
    private List<TowerPlaceHold> placeHolds = new List<TowerPlaceHold>();
    private Dictionary<int,ShowIndexPanel> showIndexPanels = new Dictionary<int, ShowIndexPanel>();
    private List<TowerInfomation> towerInfos = new List<TowerInfomation>();
    private float angle;
    private int selectIndex = 0;
    private bool isRotate = false;
    private PresetTable.Data presetData;

    public static int currentPresetIndex = -1; // 몇번 째 프리셋에 접근했는지에 대한 인덱스
    public override void Close()
    {
        base.Close();
        Release();
    }

    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        windowId = (int)WindowIds.TitleTowerPlaceEditWindow;

        Canvas.ForceUpdateCanvases();
        circleSize = new Vector3(circle.rectTransform.rect.width , circle.rectTransform.rect.height);

        closeButton.onClick.AddListener(() => manager.Open(WindowIds.TitlePresetWindow));
        saveButton.onClick.AddListener(() =>
        {
            presetData.TowerId = placeHolds.Select(x => x.TowerData == null ? -1 : x.TowerData.ID).ToList();
            DataTableManager.PresetTable.Save().Forget();
        });
    }

    public override void Open()
    {
        presetData = DataTableManager.PresetTable.Get(currentPresetIndex);

        circle.transform.eulerAngles = Vector3.zero;

        UpdateTowerHold();
        UpdateTowerList();
        RotateCircle(0);

        base.Open();
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
        for(int i = 0; i < towerList.Count; i++)
        {
            var towerInfo = Instantiate(towerInfomation, towerInfomationRoot);
            towerInfo.OnTab += Place;

            var showIndexPanel = Instantiate(this.showIndexPanel, towerInfo.transform);
            showIndexPanel.OnTab += UnPlace;

            towerInfo.Init(towerList[i].ID);
            towerInfos.Add(towerInfo);

            int curIdx = ContainPresetList(towerList[i].ID);
            showIndexPanel.Init(towerInfo);
            showIndexPanel.UpdatePlace(curIdx);

            showIndexPanels.Add(towerList[i].ID, showIndexPanel);
        }
    }

    private void UnPlace(int idx)
    {
        if (isRotate) return;
        showIndexPanels[placeHolds[idx].TowerData.ID].UpdatePlace(-1);
        placeHolds[idx].PlaceTower(null);
    }

    private void Place(TowerTable.Data data)
    {
        if (isRotate) return;
        if (!placeHolds[selectIndex].Placed())
        {
            placeHolds[selectIndex].PlaceTower(data);
            showIndexPanels[data.ID].UpdatePlace(selectIndex + 1);
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

            RectTransform rect = placeHold.GetComponent<RectTransform>();

            rect.anchoredPosition = new Vector3
            (
                Mathf.Cos(Mathf.Deg2Rad * startAngle),
                Mathf.Sin(Mathf.Deg2Rad * startAngle)
            ) * circleSize * 0.35f;

            startAngle += angle;
            rect.eulerAngles = new Vector3(0, 0, angle * i + 1);
            placeHolds.Add(placeHold);

            var towerData = DataTableManager.TowerTable.Get(presetData.TowerId[i]);
            placeHold.PlaceTower(towerData);

            int idx = i;
            placeHold.button.onClick.AddListener(() => RotateCircle(idx));
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
}
