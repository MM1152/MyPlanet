using System;
using TMPro;
using UnityEngine;

public class DebugTowerStatus : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TMP_InputField inputField;
    private int index;
    public event Action<int, float> onChangeValue;
    private void Awake()
    {
        inputField.onValueChanged.AddListener(value =>
        {
            onChangeValue?.Invoke(index, float.Parse(value));
        });
    }

    public void Init(int index)
    {
        this.index = index;
    }

    public void UpdateTitle(string msg)
    {
        titleText.text = msg;
    }

    public void UpdateInputField(float value)
    {
        if(value == 0)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);
        inputField.text = value.ToString();
    }
}
