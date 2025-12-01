using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Text;
using TMPro;
using NUnit.Framework;
using System.Runtime.CompilerServices;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private List<Tutorial> tutorials;

    [Header("Tutorial Settings")]
    [SerializeField] private GameObject tutorialBackGround;
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] private RectTransform tutorialHighLightImage;
    [SerializeField] private int startTutorialIndex = -1;
    [Header("TextAnimationSetting")]
    [SerializeField] private float wordDelay;
    [Header("References")]
    [SerializeField] private WindowManager windowManager;

    private bool isPlayWordAnimation = false;
    private StringBuilder sb = new StringBuilder();
    private Tutorial currentTutorial;
    private Image tutorialBackGroundImage; 
    private int currentTutorialIdx = -1;
    private void Awake()
    {
        tutorialHighLightImage.gameObject.SetActive(false);
        tutorialBackGround.SetActive(false);

        Variable.IsTutorialActive = false;
        Variable.IsSpawnActive = true;

        tutorialBackGroundImage = tutorialBackGround.GetComponent<Image>();
        foreach (var tutorial in tutorials)
        {
            tutorial.Init(this);
        }

        if(startTutorialIndex != -1)
        {
            Variable.IsTutorialActive = true;
            Variable.IsSpawnActive = false;
        }
            
    }

    private void OnEnable()
    {

        if (startTutorialIndex != -1)
        {
            currentTutorial = tutorials[startTutorialIndex];
            currentTutorialIdx = startTutorialIndex;
            currentTutorial.Excute();
        }
    }

    private void Update()
    {
        if(currentTutorial != null)
        {
            currentTutorial.Update();
        }

        if(currentTutorial != null && currentTutorial.IsSelectTarget && Managers.TouchManager.TouchType == TouchTypes.Tab && !isPlayWordAnimation)
        {
            ForceUpdateTutorial();
        }   
    }
    public void ForceUpdateTutorial()
    {
        currentTutorialIdx++;
        Time.timeScale = 1f;
        tutorialHighLightImage.gameObject.SetActive(false);
        tutorialBackGround.SetActive(false);

        if (currentTutorialIdx >= tutorials.Count)
        {
            currentTutorial.Exit();
            currentTutorial = null;
            tutorialText.text = string.Empty;
            tutorialBackGround.SetActive(false);
            return;
        }
        currentTutorial.Exit();
        currentTutorial = tutorials[currentTutorialIdx];
        currentTutorial.Excute();
    }
    public async UniTask WordAnimationAsync(string msg)
    {
        isPlayWordAnimation = true;
        tutorialBackGround.SetActive(true);
        sb.Clear();

        int currentStringIdx = 0;
        sb.Append(msg[0]);
        
        while(sb.Length != msg.Length)
        {
            tutorialText.text = sb.ToString();
            currentStringIdx++;
            sb.Append(msg[currentStringIdx]);
            await UniTask.Delay((int)(wordDelay * 1000f), ignoreTimeScale : true , cancellationToken: this.gameObject.GetCancellationTokenOnDestroy());
        }

        tutorialText.text = sb.ToString();
        isPlayWordAnimation = false;
    }

    public void SetTutorialBackGroundActive(bool isActive)
    {
        tutorialBackGroundImage.raycastTarget = isActive;
    }

    public void SetTutorialHighLightImagePositionAndSize(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();
        tutorialHighLightImage.gameObject.SetActive(true);
        tutorialHighLightImage.anchorMin = target.anchorMin;
        tutorialHighLightImage.anchorMax = target.anchorMax;
        tutorialHighLightImage.pivot = target.pivot;    

        Vector2 targetSize = new Vector2(target.rect.width , target.rect.height);
        Debug.Log(target.transform.position);
        Vector3 targetPosition = target.transform.position;
        
        tutorialHighLightImage.sizeDelta = targetSize;
        tutorialHighLightImage.position = targetPosition;
    }

    public void SetTutorialHighLightImagePositionAndSize(GameObject target , Vector2 sizedelta)
    {
        tutorialHighLightImage.gameObject.SetActive(true);
        tutorialHighLightImage.anchorMin = Vector2.one * 0.5f;
        tutorialHighLightImage.anchorMax = Vector2.one * 0.5f;
        tutorialHighLightImage.pivot = Vector2.one * 0.5f;


        var targetPos = new Vector3(target.transform.position.x , target.transform.position.y , -Camera.main.transform.position.z);
        targetPos = Camera.main.WorldToScreenPoint(target.transform.position);

        tutorialHighLightImage.sizeDelta = sizedelta;
        tutorialHighLightImage.position = targetPos;
    }
}
