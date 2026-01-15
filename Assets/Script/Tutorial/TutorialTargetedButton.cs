using UnityEngine;
using UnityEngine.UI;

public class TutorialTargetedButton : MonoBehaviour
{
    public int ButtonID;
    private Button button;
    private TutorialManager tutorialManager;

    private void Awake()
    {
        button = GetComponent<Button>();
    }
    private void Start()
    {
        tutorialManager = GameObject.FindWithTag(TagIds.TutorialManagerTag).GetComponent<TutorialManager>();
    }

    public void UpdateButton()
    {
        button.onClick.AddListener(OnClickButton);
    }

    private void OnClickButton()
    {
        button.onClick.RemoveListener(OnClickButton);
        tutorialManager.SetNextTutorial();
    }
}
