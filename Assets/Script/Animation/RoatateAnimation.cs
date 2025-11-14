using UnityEngine;
using UnityEngine.UIElements;

public class RoatateAnimation : MonoBehaviour
{
    private void Roatate()
    {
        transform.Rotate(new Vector3(0f, 0f, 45f) * Time.deltaTime);
    }

    private void Update()
    {
        Roatate();
    }
}
