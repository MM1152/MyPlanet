
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SoundManager : MonoBehaviour
{
    private Dictionary<AudiosId, AudioClip> audioClips = new Dictionary<AudiosId, AudioClip>();
    private List<AudioSource> allAudioSources = new List<AudioSource>();
    // private Queue<AudioSource> sfxPool = new Queue<AudioSource>();
    private AudioSource currentBgmSource;
    private AudioSource currentSfxSource;   
    private AudiosId currentBgmId = AudiosId.None;
    private AudiosId currentSfxId = AudiosId.None;
    public AudiosId CurrentBgmId => currentBgmId;  

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

        currentBgmSource = gameObject.AddComponent<AudioSource>();
        allAudioSources.Add(currentBgmSource);
        currentSfxSource = gameObject.AddComponent<AudioSource>();
        allAudioSources.Add(currentSfxSource);
    }

    public void PlaySFX(AudiosId id, float volume = 1f, bool loop = false)
    {
        if (audioClips.TryGetValue(id, out AudioClip clip))
        {
            if (currentSfxSource == null)
            {
                currentSfxSource = gameObject.AddComponent<AudioSource>();
                allAudioSources.Add(currentSfxSource);
            }

            currentSfxSource.enabled = true;
            currentSfxSource.loop = loop;
            currentSfxSource.playOnAwake = false;
            currentSfxSource.volume = volume;
            currentSfxSource.clip = null;
            currentSfxSource.PlayOneShot(clip, volume);
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
        }
        else
        {
            Debug.LogWarning($"오디오를 못찾겠군요... 그의 아이디는? {id}");
        }
    }

    public void StopBGM()
    {
        if (currentBgmSource != null)
        {
            currentBgmSource.Stop();    
        }
    }

    public void StopSFX()
    {
        if (currentSfxSource != null)
        {
            StopAudioSource(currentSfxSource);
            currentSfxId = AudiosId.None;
        }
    }

    public void StopAudioSource(AudioSource source)
    {
        if (allAudioSources.Contains(source))
        {
            source.Stop();
            ResetSource(source);
        }
    }

    public void StopAllAudioSources()
    {
        foreach (var source in allAudioSources)
        {
            StopAudioSource(source);
        }
    }

    private void ResetSource(AudioSource source)
    {
        if (source == null)
            return;

        source.playOnAwake = false;
        source.loop = false;
        source.clip = null;
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
        currentBgmId = AudiosId.None;
        currentBgmSource = null;
        currentSfxId = AudiosId.None;
        currentSfxSource = null;
        audioClips.Clear();
    }


}
