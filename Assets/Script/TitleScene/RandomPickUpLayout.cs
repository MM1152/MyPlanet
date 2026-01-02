using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
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

    public Button PickOneButton => pickOneButton;

    private List<RandomPickUpTable.Data> randomPickList;
    private List<RandomPickUpTable.Data> sortedPickList;
    private bool isPlanetPickUp = false;

    public void Init(List<RandomPickUpTable.Data> randomPickList)
    {
        this.randomPickList = randomPickList;
        sortedPickList = randomPickList;
        sortedPickList.Sort((a, b) => a.probability.CompareTo(b.probability));
        isPlanetPickUp = randomPickList[0].IsPlanetReward;

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
        popup.SetTexts("�̱⸦ �����Ͻðڽ��ϱ�?" , $"{titleText.text} �̱⸦ �����մϴ�.\n100���̾ư� �Ҹ�˴ϴ�." , "���" , "�̱�");
        popup.SetButtonAction(() => OnClickBlueButton(1).Forget(), OnClickRedButton);
    }

    public void OnClickPickTenButton()
    {
        var popup = popupManager.Open<TextPopup>(PopupIds.TextPopup);
        popup.SetTexts("�̱⸦ �����Ͻðڽ��ϱ�?", $"{titleText.text} �̱⸦ �����մϴ�.\n1000���̾ư� �Ҹ�˴ϴ�.", "���", "�̱�");
        popup.SetButtonAction(() => OnClickBlueButton(10).Forget(), OnClickRedButton);
    }

    private async UniTaskVoid OnClickBlueButton(int count)
    {
        var sucess = await FirebaseManager.Instance.UserData.CheckGoodsAsync(DataBasePaths.DiamondPath, count * 100);

        if (!sucess && !Variable.IsTutorialActive)
        {
            return;
        }

        List<UniTask> tasks = new List<UniTask>();
        if(!Variable.IsTutorialActive)
        {
            FirebaseManager.Instance.UserData.UseGoods(0, 0, count * 100).Forget();
        }
        List<RandomPickUpTable.Data> datas = new List<RandomPickUpTable.Data>();
        List<bool> isNews = new List<bool>();
        List<(bool duplication , float precData)> isDuplication = new List<(bool duplication, float precData)>();
        (UniTask task, bool isNew, (bool duplication, float precData) isDuplication) task = (new UniTask(), false, (false , 0f));

        for (int i = 0; i < count; i++)
        {
            if (isPlanetPickUp)
                datas.Add(DataTableManager.RandomPickUpTable.GetRandomDataForPlanet());
            else 
                datas.Add(DataTableManager.RandomPickUpTable.GetRandomDataForTower());


            if (datas[i].IsPlanetReward)
            {
                task = UpdatePlanetData(datas[i]);
                await Managers.Instance.WaitForLoadingAsync(task.task);
                isNews.Add(task.isNew);
                isDuplication.Add(task.isDuplication);
            }
            else
            {
                task = UpdateTowerData(datas[i]);
                await Managers.Instance.WaitForLoadingAsync(task.task);
                isNews.Add(task.isNew);
                isDuplication.Add(task.isDuplication);
            }
        }

        var window = windowManager.Open(WindowIds.TitlePickUpResultWindow);
        if (window is TitlePickUpResultWindow pickUpResultWindow)
        {
            pickUpResultWindow.SetData(datas, isNews, isDuplication);
        }

        popupManager.ForceClose();
    }

    private void OnClickRedButton()
    {
        popupManager.ForceClose();
    }

    private (UniTask, bool, (bool duplication, float precData)) UpdatePlanetData(RandomPickUpTable.Data data)
    {
        var userPlanetData = FirebaseManager.Instance.PlanetData.GetOrigin(data.connection_id);
        if(data.reward_type == 1)
        {
            //����
            if(userPlanetData.unlocked)
            {
                // �ر� �Ǿ��ִ� ����
                return (FirebaseManager.Instance.PlanetData.AddPieceCountAsync(userPlanetData.id, 10) , false, (true , 10f));
            }
            else
            {
                // �ر� �ȵǾ��ִ� ����
                return (FirebaseManager.Instance.PlanetData.UnlockPlanetAsync(userPlanetData.id) , true, (false , 0f));
            }
        }
        else
        {
            //����
            return (FirebaseManager.Instance.PlanetData.AddPieceCountAsync(userPlanetData.id, data.amount ?? 0) , false, (true , data.amount ?? 0));
        }
    }

    private (UniTask, bool, (bool duplication, float precData)) UpdateTowerData(RandomPickUpTable.Data data)
    {
        var userTowerData = FirebaseManager.Instance.TowerData.Get(data.connection_id);
        var randomOptionTable = DataTableManager.TowerRandomOptionValueTable;
        var randomOptionData = randomOptionTable.GetOptionData(userTowerData.TowerId);
        var randomOptionValue = randomOptionTable.GetRandomOptionValue(userTowerData.TowerId , userTowerData.grade);
        // 1. ���� ������ ������ �ִ� Ÿ���� ���� �� RandomOption �� ��
        // 2. �� ������ ��ü, �ƴ϶�� �������� ��ü
        // 3. ó����� Ÿ����� �׳� �ھƳ���

        if (userTowerData.Unlock)
        {
            // �� ������ �̾Ƽ� ��ü
            if(userTowerData.OptionValue < randomOptionValue.percent)
            {
                var prevOptionValue = userTowerData.OptionValue;
                return (FirebaseManager.Instance.TowerData.UpdateOptionValueAsync(userTowerData,randomOptionValue.percent), false, (true, prevOptionValue));
            }
            // �� ������ �̾Ƽ� �������� ��ü
            else
            {
                var duplicationPiece = DataTableManager.TowerDuplicationRewardTable.GetDuplicationPartCount(userTowerData.TowerId, randomOptionData.GetGradeToId(userTowerData.grade), randomOptionValue.LMH);
                return (FirebaseManager.Instance.TowerData.AddPartCountAsync(userTowerData, duplicationPiece), false, (false, duplicationPiece));
            }
        }
        else
        {
            // ó�� ���� Ÿ��
            return (FirebaseManager.Instance.TowerData.UnlockAsync(userTowerData, randomOptionValue.percent), true, (false, 0f));
        }
    }
}
