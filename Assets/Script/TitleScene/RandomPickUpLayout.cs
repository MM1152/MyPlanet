using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
public class RandomPickUpLayout : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI titleText;

    [Header("Buttons")]
    [SerializeField] private Button pickOneButton;
    [SerializeField] private Button pickTenButton;
    [SerializeField] private Button probabiltyButton;

    [Header("References")]
    [SerializeField] private PopupManager popupManager;
    [SerializeField] private WindowManager windowManager;

    private List<RandomPickUpTable.Data> randomPickList;
    private List<RandomPickUpTable.Data> sortedPickList;
    public void Init(List<RandomPickUpTable.Data> randomPickList)
    {
        this.randomPickList = randomPickList;
        sortedPickList = randomPickList;
        sortedPickList.Sort((a, b) => b.probability.CompareTo(a.probability));

        probabiltyButton.onClick.AddListener(OnClickProbabilityButton);
        pickOneButton.onClick.AddListener(OnClickPickOneButton);
        pickTenButton.onClick.AddListener(OnClickPickTenButton);

    }

    public void OnClickProbabilityButton()
    {
        var popup = popupManager.Open<RandomOptionPopup>(PopupIds.RandomOptionPopup);
        popup.SetRandomPickUpList(randomPickList);
    }

    public void OnClickPickOneButton()
    {
        var popup = popupManager.Open<TextPopup>(PopupIds.TextPopup);
        popup.SetTexts("뽑기를 진행하시겠습니까?" , "뽑기를 진행합니다.\n100다이아가 소모됩니다." , "취소" , "뽑기");
        popup.SetButtonAction(() => OnClickBlueButton(1).Forget(), OnClickRedButton);
    }

    public void OnClickPickTenButton()
    {
        var popup = popupManager.Open<TextPopup>(PopupIds.TextPopup);
        popup.SetTexts("뽑기를 진행하시겠습니까?", "뽑기를 진행합니다.\n1000다이아가 소모됩니다.", "취소", "뽑기");
        popup.SetButtonAction(() => OnClickBlueButton(10).Forget(), OnClickRedButton);
    }

    private async UniTaskVoid OnClickBlueButton(int count)
    {
        // FIX : 재화도 추가로 집어넣어야함
        List<RandomPickUpTable.Data> datas = new List<RandomPickUpTable.Data>();
        List<bool> isNews = new List<bool>();
        List<bool> isDuplication = new List<bool>();
        (UniTask task, bool isNew, bool isDuplication) task = (new UniTask(), false, false);

        for (int i = 0; i< count; i++)
        {
            datas.Add(DataTableManager.RandomPickUpTable.GetRandomDataForPlanet());

            if (datas[i].IsPlanetReward)
            {
                task = UpdatePlanetData(datas[i]);
                await Managers.Instance.WaitForLoadingAsync(task.task);
                isNews.Add(task.isNew);
                isDuplication.Add(task.isDuplication);
            }
        }

        var window = windowManager.Open(WindowIds.TitlePickUpResultWindow);
        if (window is TitlePickUpResultWindow pickUpResultWindow)
        {
            if(count == 1)
            {
                pickUpResultWindow.SetData(datas[0], isNews[0], isDuplication[0]);
            }
            else if(count == 10)
            {
                pickUpResultWindow.SetData(datas, isNews, isDuplication);
            }
        }

        popupManager.ForceClose();
    }

    private void OnClickRedButton()
    {
        popupManager.ForceClose();
    }

    private (UniTask, bool, bool) UpdatePlanetData(RandomPickUpTable.Data data)
    {
        var userPlanetData = FirebaseManager.Instance.PlanetData.GetOrigin(data.connection_id);
        if(data.reward_type == 1)
        {
            //완제
            if(userPlanetData.unlocked)
            {
                // 해금 되어있는 상태
                return (FirebaseManager.Instance.PlanetData.AddPieceCountAsync(userPlanetData.id, 10) , false, true);
            }
            else
            {
                // 해금 안되어있는 상태
                return (FirebaseManager.Instance.PlanetData.UnlockPlanetAsync(userPlanetData.id) , true, false);
            }
        }
        else
        {
            //조각
            return (FirebaseManager.Instance.PlanetData.AddPieceCountAsync(userPlanetData.id, data.amount ?? 0) , false, false);
        }
    }
}
