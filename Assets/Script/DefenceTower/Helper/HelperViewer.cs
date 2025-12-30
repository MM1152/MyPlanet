using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HelperViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI userNickNameText;
    [SerializeField] private Image userIcon;
    [SerializeField] private Image roatateTargetImage;

    private float rotationSpeed = 50f;
    private bool rotationActiveFlag = false;

    public void SetUserData(AsyncPlayerData.Data data)
    {
        userNickNameText.text = data.playerNickName;
        SetUserIconImage(data).Forget();
    }

    private async UniTaskVoid SetUserIconImage(AsyncPlayerData.Data data)
    {
        if (string.IsNullOrEmpty(data.imageUrl))
        {
            return;
        }
        userIcon.sprite = await FirebaseManager.Instance.Auth.DownLoadIconImage(new Uri(data.imageUrl));
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void RotationActive(bool active)
    {
        rotationActiveFlag = active;
    }

    private void Update()
    {
        roatateTargetImage.transform.rotation *= Quaternion.Euler(0f, 0f, rotationActiveFlag ? rotationSpeed * Time.deltaTime : 0f);
    }
}
