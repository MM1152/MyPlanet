using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class SliderValue : MonoBehaviour
{
    [SerializeField] private string FormatingString;
    [SerializeField] private TextMeshProUGUI text;
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
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
}
