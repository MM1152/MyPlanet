using UnityEngine;
using UnityEngine.UI;

public class UpgradeLayout : MonoBehaviour
{
    [SerializeField] private Image giveUpgradeTargetImage;
    [SerializeField] private Image receiveUpgradeTargetImage;

    public void ResiveUpgrade()
    {
        receiveUpgradeTargetImage.gameObject.SetActive(true);
    }

    public void GiveUpgrade()
    {
        giveUpgradeTargetImage.gameObject.SetActive(true);
    }


    public void ResetResiveUpgrade()
    {
        receiveUpgradeTargetImage.gameObject.SetActive(false);
    }

    public void ResetGiveUpgrade()
    {
        giveUpgradeTargetImage.gameObject.SetActive(false);
    }

    public void ResetImages()
    {
        receiveUpgradeTargetImage.gameObject.SetActive(false);
        giveUpgradeTargetImage.gameObject.SetActive(false);
    }
}
