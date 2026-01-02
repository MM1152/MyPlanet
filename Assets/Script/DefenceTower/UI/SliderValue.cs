using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour
{
    [SerializeField] private string FormatingString;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image fillImage;
    [SerializeField] private Sprite greenFillSprite;
    [SerializeField] private Sprite yellowFillSprite;
    [SerializeField] private Sprite redFillSprite;
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }
    
    public void UpdateSlider(int value , int maxValue)
    {
        slider.value = (float)value / maxValue;
    }

    public void UpdateSlider(int value , int maxValue, object args)
    {
        if (text != null)
        {
            text.text = string.Format(FormatingString, args);
        }

        slider.value = (float)value / maxValue;
    }


    public void UpdateSlider(int value , int maxValue , object args1 , object args2)
    {
        if (text != null)
        {
            text.text = string.Format(FormatingString, args1, args2);
        }

        slider.value = (float)value / maxValue;
    }

    public void UpdateSlider(int value, int maxValue, object args1, object args2 , object args3)
    {
        if (text != null)
        {
            text.text = string.Format(FormatingString, args1, args2, args3);
        }

        slider.value = (float)value / maxValue;
    }

    public void UpdateSlider(string msg , int value, int maxValue)
    {
        if(text != null)
        {
            text.text = msg;
        }
        slider.value = (float)value / maxValue;
    }

    public void UpdateFillImageColor()
    {
        if(fillImage ==null) return;

        if (slider.value > 0.4f)
        {
            fillImage.sprite = greenFillSprite;
        }
        else if (slider.value > 0.15f)
        {
            fillImage.sprite = yellowFillSprite;
        }
        else
        {
            fillImage.sprite = redFillSprite;
        }
    }
}
