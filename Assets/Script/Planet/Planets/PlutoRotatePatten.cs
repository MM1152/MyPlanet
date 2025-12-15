using UnityEngine;

public class PlutoRotatePatten : MonoBehaviour
{
    public float rotateSpeed = 10f;
    private void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, 0f , rotateSpeed * Time.deltaTime));
    }
}
