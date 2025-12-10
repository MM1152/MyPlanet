using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour
{
    [SerializeField] private PopupManager popupManager;
    [SerializeField] private List<Button> sideBarButtons;

    private void Start()
    {
        foreach(var bnt in sideBarButtons)
        {
            bnt.onClick.AddListener(OnClickSideBarButton);
        }
    }

    private void OnClickSideBarButton()
    {
        popupManager.Open<Popup>(PopupIds.SideBarPopup);
    }
}
