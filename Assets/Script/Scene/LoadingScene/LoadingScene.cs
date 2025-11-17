using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class LoadingScene : MonoBehaviour
{
    public TextMeshProUGUI currentProgress;
    public static string sceneId = "GameScene";
    public async UniTaskVoid Start()
    {
        if(string.IsNullOrEmpty(sceneId))
        {
            return;
        } 
        // currentProgress.text = "Firebase �ʱ�ȭ ��";
        // await FirebaseManager.Instance.WaitForInitalizedAsync();
        currentProgress.text = "Managers �ʱ�ȭ ��";
        await Managers.Instance.WaitForManagerInitalizedAsync();
        currentProgress.text = "���̺� �ҷ����� ��";
        await DataTableManager.WaitForInitalizeAsync();
        currentProgress.text = "Scene �ʱ�ȭ ��";
        await Addressables.LoadSceneAsync(sceneId).ToUniTask();
    } 


}
