using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image image;

    private void Awake()
    {
        image.fillAmount = 0f;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        image.fillAmount = 0f;
    }

    private void Update()
    {
        if (image.fillAmount >= 1f)
        {
            image.fillAmount = 0f;
        }

        image.fillAmount += Time.deltaTime * 5f;
    }
}
