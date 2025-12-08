using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class Tutorial
{
    [SerializeField] private List<GameObject> hilightPanels;
    [Header("딜레이 줘야할때")]
    [SerializeField] private float delay;
    [Header("원하는 테이블 ID 0 : 없음")]
    [SerializeField] private int stringTableId;
    [Header("타임 스케일 멈추고 싶을 때 켜기")]
    [SerializeField] private bool activeTimeScale;
    [Header("뒷배경 끄고 싶을때 켜기")]
    [SerializeField] private bool disAbleTutorialBackGorund;
    [Header("하이라이트 된 게임오브젝트 눌렀을때 넘어가게 하고싶으면 켜기 ( 딜레이 적용 )")]
    [SerializeField] private bool isSelectOnTarget;
    [Header("단순히 딜레이만 주고싶을 때 켜기")]
    [SerializeField] private bool isWaitDelay;
    [Header("버튼 조작 막고 싶을 애들 넣기 해당 튜토리얼 끝나면 켜짐")]
    [SerializeField] private List<Selectable> disableButtons;

    [Header("게임 씬 용")]
    [Header("조이스틱 키고 싶을 때 켜기")]
    [SerializeField] private bool enableJoystick;
    [Header("적 스폰하고 싶을 때 켜기")]
    [SerializeField] private bool enableSpawn;
    public bool IsSelectTarget => isSelectOnTarget;

    [SerializeField] private UnityEvent action;



    private RectTransform highlightRect;
    private TutorialManager manager;
    public float currentDelay;
    public bool IsWaitDelay => isWaitDelay;
    public bool IsNext { get; set; } = false;
    public void Init(TutorialManager manager)
    {
        this.manager = manager;
    }

    public void Excute()
    {
        if(activeTimeScale)
            Time.timeScale = 0f;

        Variable.IsSpawnActive = enableSpawn;
        Variable.IsJoyStickActive = enableJoystick;
        currentDelay = delay;
        manager.SetTutorialBackGroundActive(!disAbleTutorialBackGorund);

        for (int i = 0; i < disableButtons.Count; i++)
        {
            disableButtons[i].interactable = false;
        }

        var msg = DataTableManager.StringTable.Get(stringTableId);
        if(!string.IsNullOrEmpty(msg))
        {
            manager.WordAnimationAsync(msg).Forget();
        }
        else
        {
            manager.isPlayWordAnimation = false;
        }

        action?.Invoke();

        if (hilightPanels != null && hilightPanels.Count > 0)
        {
            foreach (var panel in hilightPanels)
            {
                var findRect = panel.GetComponent<RectTransform>();
                if(findRect != null)
                {
                    highlightRect = panel.GetComponent<RectTransform>();
                    manager.SetTutorialHighLightImagePositionAndSize(panel.GetComponent<RectTransform>());
                }
                else
                {
                    manager.SetTutorialHighLightImagePositionAndSize(panel, Vector2.one * 100f);
                }
            }
        }
    }

    public void Update()
    {
        bool isTab = Managers.TouchManager.TouchType == TouchTypes.Tab || Managers.TouchManager.TouchType == TouchTypes.LongTab;
        if (delay == 0 && isSelectOnTarget && hilightPanels.Count > 0 && isTab && Managers.TouchManager.OnTargetUI(hilightPanels[0]))
        {
            manager.ForceUpdateTutorial();
            isSelectOnTarget = false;
        }

        if(isWaitDelay)
        {
            currentDelay -= Time.deltaTime;
            if(currentDelay <= 0f)
            {
                manager.ForceUpdateTutorial();
            }
        }
        else if (isSelectOnTarget && delay != 0 && highlightRect != null 
            && Managers.TouchManager.TouchPhase == TouchPhase.Performed 
            && RectTransformUtility.RectangleContainsScreenPoint(highlightRect, Managers.TouchManager.endTouchPosition))
        {
            currentDelay -= Time.unscaledDeltaTime;
            if (currentDelay <= 0f)
            {
                isSelectOnTarget = false;
                manager.ForceUpdateTutorial();
            }
        }
    }

    public void Exit()
    {
        for(int i = 0; i < disableButtons.Count; i++)
        {
            disableButtons[i].interactable = true;
        }
    }
}
