using UnityEngine;
using UnityEngine.EventSystems;

public class SoundPlayer : MonoBehaviour, IPointerDownHandler
{
    [Header("Sound Type")]
    public SoundType soundType = SoundType.None;
    [Header("Audio ID")]
    public AudiosId audioId = AudiosId.None;
    [Header("Volume")]
    public float volume = .3f;
    [Header("Loop")]
    public bool loop = false;
    [Header("Play On Start")]
    public bool playOnStart = false;
    [Header("Touch To Play")]
    public bool touchToPlay = false;
    [Header("Touch Audio ID")]
    public AudiosId touchAudioId = AudiosId.None;
    // private void OnEnable()
    // {
    //     if (playOnStart && soundType != SoundType.None && audioId != AudiosId.None)
    //     {
    //         PlaySound();
    //         Debug.Log("사운드 플레이어가 시작과 동시에 사운드를 재생합니다.");
    //     }
    // }

    public void PlaySound()
    {
        if (audioId == AudiosId.None || soundType == SoundType.None)
        {
            Debug.LogWarning("사운드 플레이어가 재생할 사운드를 못찾겠군요...");
            return;
        }
        switch (soundType)
        {
            case SoundType.BGM:
                Managers.SoundManager.PlayBGM(audioId, volume, loop);
                break;
            case SoundType.SFX:
                Managers.SoundManager.PlaySFX(audioId, volume);
                break;
            default:
                Debug.LogWarning("사운드 플레이어가 재생할 사운드를 못찾겠군요...");
                break;
        }
    }

    public void StopSound()
    {
        if (audioId == AudiosId.None || soundType == SoundType.None)
        {
            Debug.LogWarning("사운드 플레이어가 정지할 사운드를 못찾겠군요...");
            return;
        }
        
        Managers.SoundManager.StopAllAudioSources();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (touchToPlay && touchAudioId != AudiosId.None)
        {
            Managers.SoundManager.PlaySFX(touchAudioId, volume);
        }
    }
}
