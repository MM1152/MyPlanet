using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialIdToInterctationButtons : MonoBehaviour
{
    [SerializeField] private List<Button> interactionButtons = new List<Button>();
    [SerializeField] private TutorialStep tutorialId;

    public TutorialStep GetTutorialId()
    {
        return tutorialId;
    }

    public List<Button> GetButtons()
    {
        return interactionButtons;
    }
}
