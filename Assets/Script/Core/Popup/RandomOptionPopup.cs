using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class RandomOptionPopup : Popup
{
    [SerializeField] private PriorityTextLayout probabilityText;
    [SerializeField] private Transform probabilityTextRoot;
    [SerializeField] private Button closeButton;
    private List<PriorityTextLayout> probabilityTexts = new List<PriorityTextLayout>();

    public override bool Close()
    {
        return base.Close();
    }

    public override void Init(PopupManager manager)
    {
        base.Init(manager);
        popupId = (int)PopupIds.RandomOptionPopup;
        closeButton.onClick.AddListener(() => { manager.ForceClose(); });
    }

    public override void Open()
    {
        base.Open();
    }

    public void SetRandomPickUpList(List<RandomPickUpTable.Data> randomPickUpList)
    {
        if(randomPickUpList.Count > probabilityTexts.Count)
        {
            for(int i = probabilityTexts.Count; i < randomPickUpList.Count; i++)
            {
                var priority = Instantiate(probabilityText, probabilityTextRoot);
                probabilityTexts.Add(priority);
            }
        }

        for(int i = 0; i < randomPickUpList.Count; i++)
        {
            probabilityTexts[i].gameObject.SetActive(true);
            probabilityTexts[i].UpdateTexts(randomPickUpList[i].RewardName, randomPickUpList[i].probability.ToString());
        }

        for(int i = randomPickUpList.Count; i < probabilityTexts.Count; i++)
        {
            probabilityTexts[i].gameObject.SetActive(false);
        }
    }
}
