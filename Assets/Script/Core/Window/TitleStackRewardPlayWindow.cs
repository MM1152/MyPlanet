using UnityEngine;
using UnityEngine.UI;

public class TitleStackRewardPlayWindow : Window
{
    [SerializeField] private GameObject[] rewardLayout;
    [SerializeField] private RectTransform stickBarRect;
    [SerializeField] private Image image;

    [Header("Buttons")]
    [SerializeField] private Button backButton;

    private int maxWave = 150;
    
    public override void Close()
    {
        base.Close();
    }

    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        windowId = (int)WindowIds.TitleStackRewardPlayWindow;

        backButton.onClick.AddListener(() => manager.Open(WindowIds.TitleMainWindow));
    }

    public override void Open()
    {
        base.Open();
        
        // 한 프레임 대기 후 레이아웃 배치
        StartCoroutine(PositionRewardLayoutsAfterFrame());
    }

    private System.Collections.IEnumerator PositionRewardLayoutsAfterFrame()
    {
        // 한 프레임 대기 - UI 레이아웃 완료 보장
        yield return null;
        
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
        float height = topY - bottomY - 100f;
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
    }
}
