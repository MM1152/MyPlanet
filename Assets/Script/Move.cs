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
    // 이동 속도 (도/초)   
    public float rotaspeed = 90f; // 초당 90도 회전
   //기본 이동속도 배속
   public float speed = 2f;
    //자전 속도
    public float selfRotaspeed = 10f;
    //조이스틱 입력값 저장할 변수 
    private Vector2 jotstickInput;
    //현재 각도 (도 단위)
    private float currentAngle = 0f;
    //목표 각도 (도 단위)
    private float targetAngle = 0f;
    // 이동 중인지 확인
    private bool isMoving = false;

    private void Start()
    {
        shapeRadius = Shape.transform.localScale.x / 2f;
        orbitRadius = shapeRadius + radiusPlus;

        // 초기 각도 설정 (도 단위)
        Vector3 offset = Cube.transform.position - Shape.transform.position;
        currentAngle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        targetAngle = currentAngle;

        UpdatePosition();
    }

    private void Update()
    {
        if (Cube == null || Shape == null)
            return;

        // 자전
        Cube.transform.Rotate(Vector3.up, selfRotaspeed * Time.deltaTime);

        // 새로운 조이스틱 입력이 있는 경우
        if (jotstickInput.magnitude != 0f)
        {
            // 목표 각도 계산 (도 단위)
            float newTargetAngle = Mathf.Atan2(jotstickInput.y, jotstickInput.x) * Mathf.Rad2Deg;

            // 목표 각도가 변경되었을 때만 업데이트
            if (Mathf.Abs(Mathf.DeltaAngle(targetAngle, newTargetAngle)) > 1f)
            {
                targetAngle = newTargetAngle;
                isMoving = true;
            }
        }
        else
        {
            isMoving = false;
        }

        // 현재 움직이는 중이라면 일정한 속도로 회전
        if (isMoving)
        {
            float angleDifference = Mathf.DeltaAngle(currentAngle, targetAngle);

            // 목표 각도에 도달했는지 확인
            if (Mathf.Abs(angleDifference) <= 0.1f)
            {
                currentAngle = targetAngle;
                isMoving = false;
            }
            else
            {
                // 일정한 속도로 회전 (거리에 관계없이 동일한 속도)
                float moveDirection = Mathf.Sign(angleDifference);
                float deltaAngle = rotaspeed * speed * Time.deltaTime;

                // 목표 각도를 넘어서지 않도록 제한
                if (deltaAngle > Mathf.Abs(angleDifference))
                {
                    deltaAngle = Mathf.Abs(angleDifference);
                }

                currentAngle += moveDirection * deltaAngle;
            }

            // 각도 정규화 (0 ~ 360도)
            currentAngle = (currentAngle + 360f) % 360f;

            // 새로운 위치 계산
            UpdatePosition();
        }
    }

    private void UpdatePosition()
    {
        float radianAngle = currentAngle * Mathf.Deg2Rad;
        float posX = Shape.transform.position.x + Mathf.Cos(radianAngle) * orbitRadius;
        float posY = Shape.transform.position.y + Mathf.Sin(radianAngle) * orbitRadius;

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