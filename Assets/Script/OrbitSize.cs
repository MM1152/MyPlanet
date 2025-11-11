using UnityEngine;

public class OrbitSize : MonoBehaviour
{
    public GameObject Shape;
    private float shapeRadius;
    public float radiusPlus = 2f;
    private float orbitRadius;
    //비율
    public float size = 0.3f;

    private void Update()
    {
        shapeRadius = Shape.transform.localScale.x / 2f;
        orbitRadius = (shapeRadius + radiusPlus) * size;

        this.transform.localScale = new Vector3(orbitRadius, orbitRadius);
    }


}
