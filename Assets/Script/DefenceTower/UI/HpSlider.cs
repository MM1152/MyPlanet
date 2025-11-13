using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpSlider : MonoBehaviour
{
    private readonly string HpFormat = "{0}% ({1}/{2})";

    private Slider slider;
    [SerializeField] private TextMeshProUGUI hpText;
    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void UpdateSlider(int hp , int maxHp)
    {
        if (hpText != null)
        {
              hpText.text = string.Format(HpFormat, (int)(((float)hp / maxHp) * 100), hp, maxHp);
        }

        slider.value = (float)hp / maxHp;
    }
}
