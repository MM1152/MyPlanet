using UnityEngine;

public class UpDownAnimation : MonoBehaviour
{
    [SerializeField] private Vector3 initPosition;

    public float moveSpeed = 1f;  // 진동 속도
    public float amplitude = 1f;  // 진폭 (위아래 이동 범위)

    private float time = 0f;

    private void Start()
    {
        Canvas.ForceUpdateCanvases();

        initPosition = transform.position;
    }

    private void Update()
    {
        time += moveSpeed * Time.deltaTime;

        // Cos 함수를 사용한 부드러운 위아래 움직임
        float yOffset = Mathf.Cos(time) * amplitude;
        transform.position = initPosition + new Vector3(0f, yOffset, 0f);
    }
}
