using TMPro;
using UnityEngine;

public class TextUI : MonoBehaviour
{
    private TextMeshProUGUI damageText;
    private float timer = 1.5f;
    private float currentTime = 0f;
   
    private void Awake()
    {
        damageText = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void Init(string content, Vector3 position)
    {
        damageText.text = content;
        this.transform.position = position;
    }

    private void Update()
    {
        transform.position += Vector3.up * Time.deltaTime;
        if (timer <= currentTime)
        {
            Reset();
            Managers.ObjectPoolManager.Despawn(PoolsId.TextUI, this.gameObject);            
        }
        currentTime += Time.deltaTime;
    }

    private void Reset()
    {
        damageText.text = string.Empty;
        transform.position = Vector3.zero;
        currentTime = 0f;
        SetColor(Color.white);
    }

    private void SetColor(Color color)
    {
        damageText.color = color;
    }
}
