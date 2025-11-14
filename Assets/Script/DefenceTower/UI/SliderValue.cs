using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour
{
    private readonly string Format = "{0}% ({1}/{2})";

    private Slider slider;
    [SerializeField] private TextMeshProUGUI text;
    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void UpdateSlider(int value , int maxValue)
    {
        if (text != null)
        {
            text.text = string.Format(Format, (int)(((float)value / maxValue) * 100), value, maxValue);
        }

        slider.value = (float)value / maxValue;
    }
}
