using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerPlaceHold : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI placeHoldCountText;

    private Image image;
    private bool placed = false;
    private int towerId;
    public int TowerId => towerId;

    public void Init(int index, int towerId)
    {
        image = GetComponent<Image>();

        placeHoldCountText.text = index.ToString();
        image.color = Color.gray;
        this.towerId = towerId;
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
