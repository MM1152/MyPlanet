using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class TitleDailyGiftWindow : Window
{
    [SerializeField] private DailyGiftLayout dailyGiftLayout;
    [SerializeField] private Transform dailyGiftLayoutRoot;
    [SerializeField] private Image targetedImage;
    [SerializeField] private Button backButton;

    private UserData userData;
    private List<DailyGiftLayout> dailyGiftLayouts = new List<DailyGiftLayout>();
    private const int DAILY_GIFT_COUNT = 14;

    public override void Close()
    {
        base.Close();
    }

    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        windowId = (int)WindowIds.TitleDailyGiftWindow;
        var datas = DataTableManager.DaliyGiftTable.Get();
        int count = Mathf.Min(datas.Count, DAILY_GIFT_COUNT);

        for (int i = 0; i < count; i++)
        {
            var daily = Instantiate(dailyGiftLayout, dailyGiftLayoutRoot);
            daily.SetGiftData(datas[i]);
            daily.SetInteraction(false);

            dailyGiftLayouts.Add(daily);
        }

        backButton.onClick.AddListener(() => manager.Open(WindowIds.TitleMainWindow));

        userData = FirebaseManager.Instance.UserData;
    }

    public override void Open()
    {
        base.Open();
        
        // Open될 때 위치 설정
        PositionDailyGiftsAsync().Forget();
        CheckData();
    }

    private void CheckData()
    {
        for(int i = 0; i < dailyGiftLayouts.Count; i++)
        {
            dailyGiftLayouts[i].SetInteraction(false);
            dailyGiftLayouts[i].SetCheckBox(false);
        }

        // 현재 받을 수 있는 범위 판단
        for(int i = 0; i <= userData.dailyGiftDate; i++)
        {
            if (userData.getDailyGift[i] == 1)
            {
                dailyGiftLayouts[i].SetInteraction(false);
                dailyGiftLayouts[i].SetCheckBox(true);
            }
            else if (userData.getDailyGift[i] == 0)
            {
                dailyGiftLayouts[i].SetInteraction(true);
                dailyGiftLayouts[i].SetCheckBox(false);
            }
        }
    }

    private async UniTaskVoid PositionDailyGiftsAsync()
    {
        // 한 프레임 대기
        await UniTask.Yield();

        // Canvas 강제 업데이트
        Canvas.ForceUpdateCanvases();

        // 모든 레이아웃 위치 설정
        for (int i = 0; i < dailyGiftLayouts.Count; i++)
        {
            PositionDailyGift(dailyGiftLayouts[i].transform, i);
        }
    }

    private void PositionDailyGift(Transform giftTransform, int index)
    {
        RectTransform targetRect = targetedImage.rectTransform;
        RectTransform giftRect = giftTransform.GetComponent<RectTransform>();

        float height = targetRect.rect.height;
        float cellHeight = height / 14;
        float yPos = index * cellHeight - height * 0.46f;

        // RectTransform 설정
        giftRect.anchoredPosition = new Vector2(0f, yPos);
    }
}