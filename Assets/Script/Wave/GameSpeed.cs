using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum SpeedType
{
    OneSpeed = 1,
    TwoSpeed = 2,
    // ThreeSpeed = 3
}
public class GameSpeed : MonoBehaviour
{
    [SerializeField] Button onSpeedButton;
    [SerializeField] TextMeshProUGUI speedText;  
    private static SpeedType currentSpeed = SpeedType.OneSpeed;
    public static SpeedType CurrentSpeed => currentSpeed;

    private void Awake()
    {
        if(onSpeedButton != null)
            onSpeedButton.onClick.AddListener(() =>
            {
                if(currentSpeed == SpeedType.OneSpeed)
                {
                    SetGameSpeed(SpeedType.TwoSpeed);
                }
                else if(currentSpeed == SpeedType.TwoSpeed)
                {
                    SetGameSpeed(SpeedType.OneSpeed);
                }
                // else if(currentSpeed == SpeedType.ThreeSpeed)
                // {
                //     SetGameSpeed(SpeedType.OneSpeed);
                // }
            });

        SetGameSpeed(currentSpeed);
    }

    private void SetGameSpeed(SpeedType speedType)
    {
        currentSpeed = speedType;
        speedText.text = $"x{(int)speedType}";  
        Time.timeScale = (int)speedType;
    }

    public static float ResetGameSpeed()
    {
        currentSpeed = SpeedType.OneSpeed;
        Time.timeScale = (int)currentSpeed;
        return Time.timeScale;
    }    
}
