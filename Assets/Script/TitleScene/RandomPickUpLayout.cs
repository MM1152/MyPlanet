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

        pickTenButton.interactable = false;
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
        popup.SetButtonAction(() => OnClickBlueButton().Forget(), OnClickRedButton);
    }

    public void OnClickPickTenButton()
    {
        var popup = popupManager.Open<TextPopup>(PopupIds.TextPopup);
        popup.SetTexts("뽑기를 진행하시겠습니까?", "뽑기를 진행합니다.\n1000다이아가 소모됩니다.", "취소", "뽑기");
        popup.SetButtonAction(() => OnClickBlueButton().Forget(), OnClickRedButton);
    }

    private async UniTaskVoid OnClickBlueButton()
    {
        // FIX : 재화도 추가로 집어넣어야함
        var data = DataTableManager.RandomPickUpTable.GetRandomDataForPlanet();
        (UniTask task , bool isNew) task = (new UniTask() , false);
        if(data.IsPlanetReward)
        {
            task = UpdatePlanetData(data);
            await Managers.Instance.WaitForLoadingAsync(task.task);
        }
        else if(data.IsTowerReward)
        {

        }

        var window = windowManager.Open(WindowIds.TitlePickUpResultWindow);
        if(window is TitlePickUpResultWindow pickUpResultWindow)
        {
            pickUpResultWindow.SetData(data, task.isNew);
        }
        popupManager.ForceClose();
    }

    private void OnClickRedButton()
    {
        popupManager.ForceClose();
    }

    private (UniTask, bool) UpdatePlanetData(RandomPickUpTable.Data data)
    {
        var userPlanetData = FirebaseManager.Instance.PlanetData.GetOrigin(data.connection_id);
        if(data.reward_type == 1)
        {
            //완제
            if(userPlanetData.unlocked)
            {
                // 해금 되어있는 상태
                return (FirebaseManager.Instance.PlanetData.AddPieceCountAsync(userPlanetData.id, 10) , false);
            }
            else
            {
                // 해금 안되어있는 상태
                return (FirebaseManager.Instance.PlanetData.UnlockPlanetAsync(userPlanetData.id) , true);
            }
        }
        else
        {
            //조각
            return (FirebaseManager.Instance.PlanetData.AddPieceCountAsync(userPlanetData.id, data.amount) , false);
        }
    }
}
