using UnityEngine;

public class OrbitSize : MonoBehaviour
{
    [Header("References")]
    public GameObject Shape;
    public Move moveObject;

    [Header("Settings")]
    [Range(0.1f, 2f)]
    public float sizeMultiplier = 1f; // 궤도 이미지 크기 배율

    private float lastOrbitRadius = -1f;

    private void Start()
    {
        UpdateOrbitSize();
    }

    private void Update()
    {
        // 궤도 반지름이 변경되었을 때만 업데이트
        if (moveObject != null && Mathf.Abs(lastOrbitRadius - moveObject.radiusPlus) > 0.001f)
        {
            UpdateOrbitSize();
        }
    }

    private void UpdateOrbitSize()
    {
        if (moveObject == null)
            return;

        float orbitRadius = moveObject.radiusPlus;

        // 궤도 이미지 크기 = 궤도 지름 * 배율
        float orbitDiameter = orbitRadius * 2f * sizeMultiplier;

        transform.localScale = new Vector3(orbitDiameter, orbitDiameter, 1f);

        lastOrbitRadius = orbitRadius;

        Debug.Log($"Orbit Size Updated - Radius: {orbitRadius:F2}, Diameter: {orbitDiameter:F2}");
    }

    private void OnValidate()
    {
        if (Application.isPlaying && moveObject != null)
            UpdateOrbitSize();
    }
}