using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageLayout : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI stageNameText;
    [SerializeField] private TextMeshProUGUI stageDescription;

    [Header("Images")]
    [SerializeField] private Image stageImage;

    [Header("Buttons")]
    [SerializeField] private Button leftArrowButton;
    [SerializeField] private Button rightArrowButton;

    private int stageIdx;
    public int StageIdx => stageIdx;
    private event Action<int> onClickArrow;

    public void Init(int stageIdx, Action<int> callback)
    {
        stageNameText.text = $"{stageIdx} 스테이지";
        stageDescription.text = "데이터 연결 필요";

        onClickArrow = callback;
        this.stageIdx = stageIdx;

        leftArrowButton.onClick.AddListener(() => {
            if (!FirebaseManager.Instance.UserData.isClearFirstTutorial) return;
            onClickArrow?.Invoke(stageIdx - 1);
        });
        rightArrowButton.onClick.AddListener(() => {
            if (!FirebaseManager.Instance.UserData.isClearFirstTutorial) return;
            onClickArrow?.Invoke(stageIdx + 1);
        });
    }


    public void UpdateStageLayout(bool activeLeftArrow , bool activeRightArrow , bool active)
    {
        leftArrowButton.gameObject.SetActive(activeLeftArrow);
        rightArrowButton.gameObject.SetActive(activeRightArrow);

        gameObject.SetActive(active);
    }
}
