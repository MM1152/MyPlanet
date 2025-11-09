using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{
    //이동할 방어위성 
    public GameObject Cube;
    // 행성 
    public GameObject Shape;
    //회전할 행성 반지름 
    private float shapeRadius;
    // 반지름 증가량    
    public float radiusPlus = 2f;
    // 궤도 반지름
    private float orbitRadius;
    // 이동 속도    
    public float rotaspeed = 5f;
    //자전 속도
    public float selfRotaspeed = 10f;
    //조이스틱 입력값 저장할 변수 
    private Vector2 jotstickInput;
    //현재 각도
    private float currentAngle = 0f;
    //조이스틱 각도 
    private float joystickAngle;

    float degress;

    private void Start()
    {
        shapeRadius = Shape.transform.localScale.x / 2f;
        orbitRadius = shapeRadius + radiusPlus;
        Vector3 offset = Cube.transform.position - Shape.transform.position;
        currentAngle = Mathf.Atan2(offset.y, offset.x);
        joystickAngle = currentAngle;
      
    }
    private void Update()
    {
        if (Cube == null || Shape == null)
        {
            return;
        }
        // y축을 기준으로 selfRoatspeed 속도로 자전한다.
        Cube.transform.Rotate(Vector3.up, selfRotaspeed * Time.deltaTime); //자전    
        //조이스틱 값 반영 
        if (jotstickInput.magnitude > 0.1f)
        {
            //라디안 - ~ + 라디안
            joystickAngle = Mathf.Atan2(jotstickInput.y, jotstickInput.x);
            //Vector3.SignedAngle 2중교체가아닌 한번으로만 할경우 기존 angle가 아닌 singleAngle로 계산

        }
        // 라디안 → 도 
        float currentDegree = currentAngle * Mathf.Rad2Deg;
        float joystickDegree = joystickAngle * Mathf.Rad2Deg;

        // 도 → 라디안 // moveTowardsAngle 도 사용가능 
        currentAngle = Mathf.LerpAngle(
            currentDegree,
            joystickDegree,
            Time.deltaTime * rotaspeed
        ) * Mathf.Deg2Rad;

        //새로운 위치 계산 
        float posX = Shape.transform.position.x + orbitRadius * Mathf.Cos(currentAngle);
        float posY = Shape.transform.position.y + orbitRadius * Mathf.Sin(currentAngle);

        //방어위성 위치 업데이트 
        Cube.transform.position = new Vector3(posX, posY, Cube.transform.position.z);

    }



    public void MoveCube(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jotstickInput = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            jotstickInput = Vector2.zero;
        }
    }
}
