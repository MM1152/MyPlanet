using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SoundManager : MonoBehaviour
{
    private Dictionary<AudiosId, AudioClip> audioClips = new Dictionary<AudiosId, AudioClip>();
    private List<AudioSource> allAudioSources = new List<AudioSource>();
    private Queue<AudioSource> sfxPool = new Queue<AudioSource>();
    private AudioSource currentBgmSource;
    private AudiosId currentBgmId = AudiosId.None;

    public async UniTask Init()
    {
        var assets = await Addressables.LoadAssetsAsync<AudioClip>(AddressableLabelIds.AudiosIds);

        foreach (var asset in assets)
        {
            var id = AddressableNames.GetAudiosId(asset.name);
            if (id != AudiosId.None)
            {
                audioClips.Add(id, asset);
            }
            else
            {
                Debug.Log($"파일 로드가 되지않아요.. 에셋의 이름은? :{asset.name}");
            }
        }
    }

    public void PlaySFX(AudiosId id, float volume = 1f)
    {
        if (audioClips.TryGetValue(id, out AudioClip clip))
        {
            AudioSource audioSource;
            if (sfxPool.Count > 0)
            {
                audioSource = sfxPool.Dequeue();
            }
            else
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                allAudioSources.Add(audioSource);
            }

            audioSource.enabled = true;
            audioSource.loop = false;
            audioSource.playOnAwake = false;
            audioSource.volume = volume;
            audioSource.clip = null;

            audioSource.PlayOneShot(clip, volume);
            ReturnAudioSourcePool(audioSource, clip.length).Forget();
        }
        else
        {
            Debug.LogWarning($"오디오를 못찾겠군요... 그의 아이디는? {id}");
        }
    }

    public void PlayBGM(AudiosId id, float volume = 1f, bool loop = true)
    {
        if (currentBgmId == id && currentBgmSource != null && currentBgmSource.isPlaying)
        {
            return;
        }

        if (currentBgmSource != null)
        {
            StopAudioSource(currentBgmSource);
        }

        if (audioClips.TryGetValue(id, out AudioClip clip))
        {
            if (currentBgmSource == null)
            {
                currentBgmSource = gameObject.AddComponent<AudioSource>();
                allAudioSources.Add(currentBgmSource);
            }

            currentBgmSource.enabled = true;
            currentBgmSource.loop = loop;
            currentBgmSource.volume = volume;
            currentBgmSource.clip = clip;
            currentBgmId = id;
            currentBgmSource.Play();
            Debug.Log($"BGM 재생 시작 : {id}");
        }
        else
        {
            Debug.LogWarning($"오디오를 못찾겠군요... 그의 아이디는? {id}");
        }
    }

    public void StopAudioSource(AudioSource source)
    {
        if (allAudioSources.Contains(source))
        {
            source.Stop();
            ResetSource(source);
            if (source == currentBgmSource)
            {
                currentBgmId = AudiosId.None;
                currentBgmSource = null;
            }
            if (sfxPool.Contains(source))
            {
                sfxPool.Enqueue(source);
            }
        }
    }

    public void StopAllAudioSources()
    {
        Debug.Log("Stop All Audio Sources 호출됨");
        foreach (var source in allAudioSources)
        {
            StopAudioSource(source);
        }
    }

    private async UniTask ReturnAudioSourcePool(AudioSource source, float delay)
    {
        await UniTask.Delay((int)(delay * 1000), true);
        if (source == null)
            return;
        if (source == currentBgmSource)
            return;

        if (!allAudioSources.Contains(source))
            return;

        ResetSource(source);
        sfxPool.Enqueue(source);
    }

    private void ResetSource(AudioSource source)
    {
        if (source == null)
            return;

        source.playOnAwake = false;
        source.loop = false;
        source.clip = null;
        source.pitch = 1f;
        source.volume = 1f;
        source.enabled = false;
    }

    private void OnDestroy()
    {
        foreach (var source in allAudioSources)
        {
            if (source != null)
                Destroy(source);
        }
        allAudioSources.Clear();
        sfxPool.Clear();
        currentBgmSource = null;
        audioClips.Clear();
    }


}
