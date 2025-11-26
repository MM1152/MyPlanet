using UnityEngine;
using UnityEngine.UI;

public class DebugScene : MonoBehaviour
{
    [Header ("Buttons")]
    public Button openDebugTowerPlaceWindowButton;

    [Header ("References")]
    public WindowManager windowManager;

    public void Start()
    {
        openDebugTowerPlaceWindowButton.onClick.AddListener(() => windowManager.Open(WindowIds.DebugTowerPlaceWIndow));
    }
}
