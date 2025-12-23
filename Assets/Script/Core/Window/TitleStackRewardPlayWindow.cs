using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class TitleStackRewardPlayWindow : Window
{
    [SerializeField] private GameObject[] rewardLayout;
    [SerializeField] private GameObject[] isClearImage;
    [SerializeField] private RectTransform stickBarRect;
    [SerializeField] private Image image;

    [Header("Buttons")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button[] rewardButtons;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI[] rewardValueText;
    [SerializeField] private TextMeshProUGUI[] rewardWaveText;
    [SerializeField] private TextMeshProUGUI curUserWaveText;

    private UserData userData;
    private int maxWave = 150;
    private float heightPercent;
    private ClearWaveRewardTable clearWaveRewardTable;

    private List<ClearWaveRewardTable.Data> datas = new List<ClearWaveRewardTable.Data>();

    public override void Close()
    {
        base.Close();
    }

    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        windowId = (int)WindowIds.TitleStackRewardPlayWindow;
        userData = FirebaseManager.Instance.UserData;

        backButton.onClick.AddListener(() => manager.Open(WindowIds.TitleMainWindow));
        clearWaveRewardTable = DataTableManager.ClearWaveRewardTable;

        datas.Add(clearWaveRewardTable.Get(20001));
        rewardValueText[0].text = datas[0].Num.ToString();
        rewardWaveText[0].text = datas[0].Wave.ToString();

        datas.Add(clearWaveRewardTable.Get(20002));
        rewardValueText[1].text = datas[1].Num.ToString();
        rewardWaveText[1].text = datas[1].Wave.ToString();

        datas.Add(clearWaveRewardTable.Get(20003));
        rewardValueText[2].text = datas[2].Num.ToString();
        rewardWaveText[2].text = datas[2].Wave.ToString();

        rewardButtons[0].onClick.AddListener(async () => await Managers.Instance.WaitForLoadingAsync(FirebaseManager.Instance.UserData.GetGoods(datas[0].Num)));
        rewardButtons[1].onClick.AddListener(async () => await Managers.Instance.WaitForLoadingAsync(FirebaseManager.Instance.UserData.GetGoods(datas[1].Num)));
        rewardButtons[2].onClick.AddListener(async () => await Managers.Instance.WaitForLoadingAsync(FirebaseManager.Instance.UserData.GetGoods(datas[2].Num)));

    }

    public override void Open()
    {
        base.Open();

        PositionRewardLayoutsAfterFrame().Forget();
    }

    private void CheckData()
    {
        var stackRewards = userData.stackRewards;
        for(int i = 0; i < stackRewards.Length; i++)
        {
            if (stackRewards[i] == 0)
            {
                isClearImage[i].SetActive(false);
            }
            if(stackRewards[i] == 1)
            {
                rewardButtons[i].interactable = false;
                isClearImage[i].SetActive(true);
            }
        }

        curUserWaveText.text = userData.clearWaveCount.ToString();
        image.fillAmount = userData.clearWaveCount / (float)maxWave * heightPercent; 

        for(int i = 0; i < stackRewards.Length; i++)
        {
            if(stackRewards[i] == 1 && userData.clearWaveCount >= datas[i].Wave)
            {
                rewardButtons[i].interactable = true;
            }
        }
    }

    private async UniTaskVoid PositionRewardLayoutsAfterFrame()
    {
        // 한 프레임 대기 - UI 레이아웃 완료 보장
        await UniTask.WaitForEndOfFrame();
        
        // Canvas 및 레이아웃 강제 업데이트
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(stickBarRect);
        
        // StickBar의 월드 코너 좌표 가져오기
        Vector3[] corners = new Vector3[4];
        stickBarRect.GetWorldCorners(corners);
        
        // corners[0] = 왼쪽 하단, corners[1] = 왼쪽 상단
        // corners[2] = 오른쪽 상단, corners[3] = 오른쪽 하단
        float bottomY = corners[0].y;
        float topY = corners[1].y;
        float height = topY - bottomY;
        heightPercent = (height - 100) / height;
        height = height - 100;

        float centerX = stickBarRect.position.x;
        
        Debug.Log($"StickBar - Height: {height}, Bottom: {bottomY}, Top: {topY}");
        
        // 웨이브 50, 100, 150 위치 계산 (하단부터 비례적으로)
        float wave50Y = bottomY + (height * (50f / maxWave));
        float wave100Y = bottomY + (height * (100f / maxWave));
        float wave150Y = bottomY + (height * (150f / maxWave));
        
        // rewardLayout 배치
        rewardLayout[0].transform.position = new Vector3(centerX, wave50Y, 0);
        rewardLayout[1].transform.position = new Vector3(centerX, wave100Y, 0);
        rewardLayout[2].transform.position = new Vector3(centerX, wave150Y, 0);
        
        Debug.Log($"Reward Layout Positions:");
        Debug.Log($"  Wave 50  (1/3): {wave50Y}");
        Debug.Log($"  Wave 100 (2/3): {wave100Y}");
        Debug.Log($"  Wave 150 (3/3): {wave150Y}");

        CheckData();
    }

}
