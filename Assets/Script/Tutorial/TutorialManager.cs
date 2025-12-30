using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Threading;
using System;

public enum TutorialStep
{
    None,
    Preset,
    Stage1,
    Stage1Enter,
    PickUp,
    PickUp2,
    Book,
    Book1,
    Stage2,
}

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private TutorialStep currentTutorialStep;


    [Header("Tutorial Text")]
    [SerializeField] private List<Transform> tutorialTextPositions;
    [SerializeField] private GameObject tutorialTextPanel;
    [SerializeField] private Image tutorialBackGround;
    [SerializeField] private Image tutoriamTextEndImage;
    [SerializeField] private Image tutorialTouchPanel;

    private List<Tutorial> curTutorialList;
    private AudioSource audioSource;

    public bool Skip { get; set; }
    public float rotateSpeed = 50f;
    public TextMeshProUGUI tutorialText;

    private Dictionary<TutorialStep, List<Tutorial>> tutorials = new Dictionary<TutorialStep, List<Tutorial>>()
    {
        { TutorialStep.Preset, new List<Tutorial> { new PresetWindowTutorial(), new PresetWindowTutorial2(), new PresetWindowTutorial3() } },
        { TutorialStep.Stage1Enter, new List<Tutorial> { new Stage1Enter1(), new Stage1Enter2(), new Stage1Enter3()} },
        { TutorialStep.Stage1, new List<Tutorial> { new Stage1Tutorial1()  , new Stage1Tutorial2(), new Stage1Tutorial3(), new Stage1Tutorial4() , new Stage1Tutorial5()} },
        { TutorialStep.PickUp, new List<Tutorial> { new RandomPickUpTutorial1() , new RandomPickUpTutorial2() , new RandomPickUpTutorial3() } },
        { TutorialStep.PickUp2, new List<Tutorial> { new TowerRandomPickUp1() , new TowerRandomPickUp2() } },
        { TutorialStep.Book, new List<Tutorial> { new BookTutorial1() , new BookTutorial2(), new BookTutorial3() } },
        { TutorialStep.Book1, new List<Tutorial> { new BookTutorial4() , new BookTutorial5()}},
        { TutorialStep.Stage2, new List<Tutorial> { new Stage2Tutorial() }},
    };

    [SerializeField] private List<TutorialDisAbleButtons> tutorialDisableButtons;

    private Tutorial curTutorial;
    private TutorialDisAbleButtons curTutorialDisableButtons;
    private int curIdx;
    public bool CanPlayNextTutorial { get; set; }

    public CancellationTokenSource TutorialCtr => tutorialCtr;
    private CancellationTokenSource tutorialCtr;

    private bool init = false;
    private float textSkipDelay = 0.05f;
    private void Awake()
    {
        if(!init)
            Init();
    }

    private void Init()
    {
        foreach(var key in tutorials.Keys)
        {
            foreach(var tutorial in tutorials[key])
            {
                tutorial.Init(this);
            }
        }

        init = true;
        gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 6f;
    }


    public void InitTutorial(TutorialStep tutorialId)
    {
        if (!init) Init();

        curTutorialDisableButtons = null;
        gameObject.SetActive(true); 
        curTutorialList = tutorials[tutorialId];
        currentTutorialStep = tutorialId;
        curIdx = 0;
        for(int i = 0; i < tutorialDisableButtons.Count; i++)
        {
            if(tutorialId == tutorialDisableButtons[i].tutorialStep)
            {
                curTutorialDisableButtons = tutorialDisableButtons[i];
                for(int j = 0; j < curTutorialDisableButtons.disAbleButtons.Count; j++)
                {
                    curTutorialDisableButtons.disAbleButtons[j].interactable = false;
                }
                break;
            }
        }        

        Variable.IsTutorialActive = true;

        SetNextTutorial();
    }

    public void SetNextTutorial()
    {
        textSkipDelay = 0.05f;

        tutorialTouchPanel.transform.localScale = Vector3.one;
        StopSound();
        if (curIdx >= curTutorialList.Count)
        {
            EndTutorials();
            return;
        }

        if(tutorialCtr != null)
        {
            tutorialCtr?.Cancel();
            tutorialCtr?.Dispose();
        }
        tutorialCtr = new CancellationTokenSource();

        SetActiveTouchPanel(false);
        SetActiveTutorialTextArea(false);
        SetActiveTutorialTextEndImage(false);
        tutorialTouchPanel.transform.SetParent(transform);  

        curTutorial?.TutorialExit();
        curTutorial = curTutorialList[curIdx++];
        curTutorial?.TutorialEnter();
    }

    public void Update()
    {
        if(CanPlayNextTutorial && Managers.TouchManager.TouchType == TouchTypes.Tab)
        {
            SetNextTutorial();
        }

        if(curTutorial != null && curTutorial.isPlayTexts)
        {
            textSkipDelay -= Time.unscaledDeltaTime;
        }

        if (textSkipDelay < 0 && !Skip && tutorialTextPanel.activeSelf && !GetActiveTutorialTextEndImage() && Managers.TouchManager.TouchType == TouchTypes.Tab)
        {
            Skip = true;
            textSkipDelay = 0.05f;
        }

        curTutorial?.TutorialUpdate();

        if(tutorialTouchPanel.gameObject.activeSelf)
        {
            tutorialTouchPanel.rectTransform.rotation *= Quaternion.Euler(0, 0, rotateSpeed * Time.unscaledDeltaTime);
        }
    }

    public void SetPrevTutorial()
    {
        if(curIdx - 2 >= 0)
        {
            curIdx -= 2;
        }
        else
        {
            EndTutorials();
            return;
        }
        SetNextTutorial();
    }


    public void EndTutorials()
    {
        curTutorial?.TutorialExit();
        tutorialTouchPanel.transform.parent = transform;
        gameObject.SetActive(false);

        Variable.IsTutorialActive = false;

        if(curTutorialDisableButtons != null)
        {
            for (int i = 0; i < curTutorialDisableButtons.disAbleButtons.Count; i++)
            {
                curTutorialDisableButtons.disAbleButtons[i].interactable = true;
            }
        }
    }

    public void SetTextAreaPosition(int idx)
    {
        tutorialTextPanel.transform.parent = tutorialTextPositions[idx];
        tutorialTextPanel.transform.localPosition = Vector3.zero;
    }

    public void SetTouchPanelPosition(Vector3 pos)
    {
        tutorialTouchPanel.transform.position = pos;
    }

    public void SetTouchPlanelParent(Transform target)
    {
        tutorialTouchPanel.transform.SetParent(target , true);
        tutorialTouchPanel.transform.localPosition = Vector3.zero;
        tutorialTouchPanel.transform.localScale = Vector3.one;
    }

    public void SetActiveTouchPanel(bool active)
    {
        tutorialTouchPanel.gameObject.SetActive(active);
    }

    public void SetActiveTutorialTextEndImage(bool isActive)
    {
        tutorialTouchPanel.gameObject.SetActive(true);
        tutoriamTextEndImage.gameObject.SetActive(isActive);
    }

    public void SetTutorialBackGround(bool raycastAble)
    {
        tutorialBackGround.raycastTarget = raycastAble;
    }

    public bool GetActiveTutorialTextEndImage()
    {
        return tutoriamTextEndImage.gameObject.activeSelf;
    }
    
    public void SetActiveTutorialTextArea(bool active)
    {
        tutorialTextPanel.SetActive(active);
    }

    private void OnDestroy()
    {
        tutorialCtr?.Cancel();
        tutorialCtr?.Dispose();
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void StopSound()
    {
        audioSource.Stop();
    }
}

[Serializable]   
public class TutorialDisAbleButtons
{
    public TutorialStep tutorialStep;
    public List<Selectable> disAbleButtons;
}

