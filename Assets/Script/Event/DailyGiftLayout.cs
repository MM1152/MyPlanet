using Cysharp.Threading.Tasks;
using NUnit.Framework.Constraints;
using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyGiftLayout : MonoBehaviour
{
    [SerializeField] private Image giftImage;
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private Button button;
    [SerializeField] private Image checkBox;

    [SerializeField] private PopupManager popupManager;

    private DailyGiftTable.Data data;
    private Action<int> callback;
    private int idx;
    private void Awake()
    {
        popupManager = GameObject.FindWithTag(TagIds.PopupManager).GetComponent<PopupManager>();

        button.onClick.AddListener(() =>
        {
            SaveDataAsync().Forget();
        });
        
    }


    public void SetGiftData(DailyGiftTable.Data data , int idx , Action<int> callback)
    {
        this.data = data;
        
        giftImage.sprite = data.ItemData.ItemImage;
        dayText.text = "Day " + data.ID.ToString();
        valueText.text = data.Num.ToString();

        this.callback = callback;
        this.idx = idx;
    }

    public void SetInteraction(bool active)
    {
        button.interactable = active;
    }

    public void SetCheckBox(bool active)
    {
        checkBox.gameObject.SetActive(active);
    }

    private async UniTask SaveDataAsync()
    {
        UniTask task = default;
        Sprite itemImage = data.ItemData.ItemImage;
        if (data.Type == 240001)
        {
            task = FirebaseManager.Instance.UserData.GetGoods(0, data.Num, 0);
        }
        else if (data.Type== 240002)
        {
            task = FirebaseManager.Instance.UserData.GetGoods(data.Num, 0, 0);
        }
        else if (data.Type == 240003)
        {
            task = FirebaseManager.Instance.UserData.GetGoods(0, 0, data.Num);
        }
        else if(data.Type == 2460045)
        {
            var randomPlanetData = DataTableManager.PlanetTable.GetRandomPlaentData(new char[] { 'A', 'B' });
            var userPlanetData = FirebaseManager.Instance.PlanetData.GetOrigin(randomPlanetData.ID);
            task = FirebaseManager.Instance.PlanetData.AddPieceCountAsync(randomPlanetData.ID , data.Num);

            itemImage = DataTableManager.ItemTable.GetDataToPlenetId(randomPlanetData.ID).ItemImage;
        }

        await Managers.Instance.WaitForLoadingAsync(task);

        var popup = popupManager.GetPopup(PopupIds.DailyGiftPopup);
        if (popup is DailyGiftPopup dailyGiftPopup)
        {
            dailyGiftPopup.SetData(itemImage, data.Num.ToString());
            popupManager.Open<DailyGiftPopup>(PopupIds.DailyGiftPopup);
        }

        callback?.Invoke(idx);
    }
}
