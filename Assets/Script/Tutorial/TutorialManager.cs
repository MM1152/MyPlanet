using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Text;
using TMPro;
using NUnit.Framework;
using System.Runtime.CompilerServices;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;

[HideInInspector]
[Serializable]
public class TutorialList
{
    public List<Tutorial> data;
}

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private List<TutorialList> tutorials;   

    [Header("Tutorial Settings")]
    [SerializeField] private GameObject tutorialBackGround;
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] private RectTransform tutorialHighLightImage;
    [SerializeField] private int startTutorialIndex = -1;
    [SerializeField] private List<int> nextTutorialStartIndex;
    [Header("TextAnimationSetting")]
    [SerializeField] private float wordDelay;
    [Header("References")]
    [SerializeField] private WindowManager windowManager;
    [SerializeField] private WaveManager waveManager;

    public bool isPlayWordAnimation = false;
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
            foreach(var tuto in tutorial.data)
            {
                tuto.Init(this);
            }
        }

        if (waveManager != null && nextTutorialStartIndex != null)
        {
            waveManager.NextTutorialWaveIndex = nextTutorialStartIndex[0];
        }

        if(startTutorialIndex != -1)
        {
            Variable.IsTutorialActive = true;
            Variable.IsSpawnActive = false;
        }
    }

    private async UniTaskVoid Start()
    {
        await UniTask.Delay(100 , cancellationToken : gameObject.GetCancellationTokenOnDestroy());
        if (startTutorialIndex != -1)
        {
            currentTutorial = tutorials[0].data[startTutorialIndex];
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
            currentTutorial?.Exit();
            currentTutorial = null;
            tutorialText.text = string.Empty;
            tutorialBackGround.SetActive(false);
            return;
        }
        isPlayWordAnimation = true;
        currentTutorial?.Exit();
        currentTutorial = tutorials[0].data[currentTutorialIdx];
        currentTutorial?.Excute();
    }
    public async UniTask WordAnimationAsync(string msg)
    {
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
        tutorialHighLightImage.anchorMin = Vector2.one * 0.5f;
        tutorialHighLightImage.anchorMax = Vector2.one * 0.5f;
        tutorialHighLightImage.pivot = Vector2.one * 0.5f;

        Vector2 targetSize = new Vector2(target.rect.width , target.rect.height);
        Debug.Log(target.position);
        tutorialHighLightImage.sizeDelta = targetSize;
        tutorialHighLightImage.position = target.position;
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

    public void SetSectorTutorial(int wave)
    {
        currentTutorialIdx = -1;
        ForceUpdateTutorial();

        if (waveManager != null && nextTutorialStartIndex != null)
        {
            waveManager.NextTutorialWaveIndex = nextTutorialStartIndex[1];
        }
    }
  
}
