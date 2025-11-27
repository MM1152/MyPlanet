using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{    
    public GameObject Cube;
 
    public GameObject Shape;
 
    private float shapeRadius;
 
    public float radiusPlus = 2f;
 
    private float orbitRadius;
 
    public float rotaspeed = 90f; // 초당 90도 회전
 
   public float speed = 2f;
 
    public float selfRotaspeed = 10f;
 
    private Vector2 jotstickInput;
 
    private float currentAngle = 0f;
 
    private float targetAngle = 0f;
 
    private bool isMoving = false;

    private void Start()
    {
        shapeRadius = Shape.transform.localScale.x / 2f;
        orbitRadius = shapeRadius + radiusPlus;
 
        Vector3 offset = Cube.transform.position - Shape.transform.position;
        currentAngle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        targetAngle = currentAngle;

        UpdatePosition();
    }

    private void Update()
    {
        if (Cube == null || Shape == null)
            return;

        Cube.transform.Rotate(Vector3.up, selfRotaspeed * Time.deltaTime);

        if (jotstickInput.magnitude != 0f)
        {

            float newTargetAngle = Mathf.Atan2(jotstickInput.y, jotstickInput.x) * Mathf.Rad2Deg;

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

        if (isMoving)
        {
            float angleDifference = Mathf.DeltaAngle(currentAngle, targetAngle);

            if (Mathf.Abs(angleDifference) <= 0.1f)
            {
                currentAngle = targetAngle;
                isMoving = false;
            }
            else
            {

                float moveDirection = Mathf.Sign(angleDifference);
                float deltaAngle = rotaspeed * speed * Time.deltaTime;

                if (deltaAngle > Mathf.Abs(angleDifference))
                {
                    deltaAngle = Mathf.Abs(angleDifference);
                }

                currentAngle += moveDirection * deltaAngle;
            }

            currentAngle = (currentAngle + 360f) % 360f;

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