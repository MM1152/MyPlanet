using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class LoadingScene : MonoBehaviour
{
    public TextMeshProUGUI currentProgress;
    public static string sceneId = "TitleScene";
    public async UniTaskVoid Start()
    {
        if(string.IsNullOrEmpty(sceneId))
        {
            return;
        } 
        //currentProgress.text = "Firebase 초기화 중";
        //await FirebaseManager.Instance.WaitForInitalizedAsync();
        currentProgress.text = "Managers 초기화 중";
        await Managers.Instance.WaitForManagerInitalizedAsync();
        currentProgress.text = "테이블 불러오는 중";
        await DataTableManager.WaitForInitalizeAsync();
        currentProgress.text = "Scene 초기화 중";
        await Addressables.LoadSceneAsync(sceneId).ToUniTask();
    } 


}
