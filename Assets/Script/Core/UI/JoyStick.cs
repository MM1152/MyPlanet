using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;

public class JoyStick : MonoBehaviour
{
    // 조이스틱 onoff를 위한 캔버스 그룹    
    private CanvasGroup canvasGroup;
    //조이스틱 이동반경 제한을 위한 중심점과 반경  
    public OnScreenStick onScreenStickq;
    // 조이스틱의 피봇(배경) 값 설정용 
    public RectTransform pivot;
    // 핸들위치 초기화용 
    public RectTransform handle;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    // 초기화 
    private void Start()
    {
        //조이스틱의 반경을 기준으로 범위이미지 사이즈 조정 
        float range = onScreenStickq.movementRange;
        pivot.sizeDelta = new Vector2(range * 2, range * 2);

        //초기에는 조이스틱이 보이지 않도록 설정 
         HideJoystick();
    }
    //조이스틱 보이기/숨기기 메서드 
    public void ShowJoystick(Vector3 inputPos)
    {
        //조이스틱의 위치를 터치한 위치로 이동
        this.transform.position = inputPos;
        //캔버스 그룹의 알파값을 1로 변경해서 눈에 보이게함 
        canvasGroup.alpha = 1;
        // 캔버스 그룹이 레이캐스트를 받도록 설정   
        canvasGroup.blocksRaycasts = true;
    }

    //조이스틱 보이기/숨기기 메서드 
    public void HideJoystick()
    {
        //캔버스 그룹의 알파값을 0으로 변경해서 눈에 안보이게함 
        canvasGroup.alpha = 0;
        // 캔버스 그룹이 레이캐스트를 받지 않도록 설정   
        canvasGroup.blocksRaycasts = false;
        //핸들을 중앙으로 초기화
        handle.transform.position = pivot.position;
    }
}







