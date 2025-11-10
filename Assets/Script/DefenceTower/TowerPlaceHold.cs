using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerPlaceHold : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI placeHoldCountText;

    private Image image;
    private bool placed = false;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Init(int index)
    {
        placeHoldCountText.text = index.ToString();
        image.color = Color.gray;
    }

    public void SetPlace()
    {
        placed = true;
        image.color = Color.green;
    }
    public bool GetPlaced()
    {
        return placed;
    }


}
