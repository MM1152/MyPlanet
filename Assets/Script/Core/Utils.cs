using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Utils
{
    private static Rect screenRect;
    static Utils()
    {
        var camera = Camera.main;
        var zDistance = Mathf.Abs(camera.transform.position.z);

        var bottomLeft = camera.ScreenToWorldPoint(new Vector3(0, 0, zDistance));
        var topRight = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, zDistance));

        screenRect = new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
    }

    public static bool IsPointerOverUI(Vector2 screenPosition)
    {
        var pointerData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        return results.Count > 0;
    }

    public static int ClampIndex(int index , int count)
    {
        if (index < 0)
            index = index + count;
        else if (index >= count)
            index = index % count;

        return index;
    }

    public static string FormatText(string text, params (string key, string value)[] replacements)
    {
        foreach (var (key, value) in replacements)
        {
            text = text.Replace($"[{key}]", value);
        }
        return text;
    }

    public static Rect GetScreenBounds()
    {
        return screenRect;
    }

    public static void Suffle<T>(IList<T> target)
    {
        int n = target.Count;
        System.Random rnd = new System.Random();
        while (n > 1)
        {
            int k = rnd.Next(n--);
            T temp = target[n];
            target[n] = target[k];
            target[k] = temp;
        }
    }

    /// <summary>
    /// 두 개의 AudioClip을 순차적으로 합쳐서 하나의 AudioClip으로 만듭니다.
    /// </summary>
    /// <param name="clip1">첫 번째 오디오 클립</param>
    /// <param name="clip2">두 번째 오디오 클립</param>
    /// <param name="name">합쳐진 클립의 이름</param>
    /// <returns>합쳐진 AudioClip</returns>
    public static AudioClip CombineAudioClips(AudioClip clip1, AudioClip clip2, string name = "CombinedClip")
    {
        if (clip1 == null || clip2 == null)
        {
            Debug.LogWarning("CombineAudioClips: 하나 이상의 AudioClip이 null입니다.");
            return clip1 ?? clip2;
        }

        // 두 클립의 주파수가 다르면 경고
        if (clip1.frequency != clip2.frequency)
        {
            Debug.LogWarning($"CombineAudioClips: 클립들의 주파수가 다릅니다. ({clip1.frequency} vs {clip2.frequency})");
        }

        // 두 클립의 채널 수가 다르면 경고
        if (clip1.channels != clip2.channels)
        {
            Debug.LogWarning($"CombineAudioClips: 클립들의 채널 수가 다릅니다. ({clip1.channels} vs {clip2.channels})");
        }

        // 더 높은 주파수와 채널 수를 사용
        int frequency = Mathf.Max(clip1.frequency, clip2.frequency);
        int channels = Mathf.Max(clip1.channels, clip2.channels);

        // 총 샘플 수 계산
        int clip1Samples = clip1.samples;
        int clip2Samples = clip2.samples;
        int totalSamples = clip1Samples + clip2Samples;

        // 새로운 AudioClip 생성
        AudioClip combinedClip = AudioClip.Create(name, totalSamples, channels, frequency, false);

        // 첫 번째 클립 데이터 가져오기
        float[] clip1Data = new float[clip1Samples * clip1.channels];
        clip1.GetData(clip1Data, 0);

        // 두 번째 클립 데이터 가져오기
        float[] clip2Data = new float[clip2Samples * clip2.channels];
        clip2.GetData(clip2Data, 0);

        // 합쳐진 데이터 배열 생성
        float[] combinedData = new float[totalSamples * channels];

        // 첫 번째 클립 데이터 복사 (채널 수 맞춤)
        for (int i = 0; i < clip1Samples; i++)
        {
            for (int ch = 0; ch < channels; ch++)
            {
                int sourceIndex = i * clip1.channels + Mathf.Min(ch, clip1.channels - 1);
                int targetIndex = i * channels + ch;
                if (sourceIndex < clip1Data.Length && targetIndex < combinedData.Length)
                {
                    combinedData[targetIndex] = clip1Data[sourceIndex];
                }
            }
        }

        // 두 번째 클립 데이터 복사 (채널 수 맞춤)
        for (int i = 0; i < clip2Samples; i++)
        {
            for (int ch = 0; ch < channels; ch++)
            {
                int sourceIndex = i * clip2.channels + Mathf.Min(ch, clip2.channels - 1);
                int targetIndex = (clip1Samples + i) * channels + ch;
                if (sourceIndex < clip2Data.Length && targetIndex < combinedData.Length)
                {
                    combinedData[targetIndex] = clip2Data[sourceIndex];
                }
            }
        }

        // 합쳐진 데이터를 새 클립에 설정
        combinedClip.SetData(combinedData, 0);

        return combinedClip;
    }

    /// <summary>
    /// 여러 개의 AudioClip을 순차적으로 합쳐서 하나의 AudioClip으로 만듭니다.
    /// </summary>
    /// <param name="clips">합칠 오디오 클립들</param>
    /// <param name="name">합쳐진 클립의 이름</param>
    /// <returns>합쳐진 AudioClip</returns>
    public static AudioClip CombineMultipleAudioClips(AudioClip[] clips, string name = "CombinedClip")
    {
        if (clips == null || clips.Length == 0)
        {
            Debug.LogWarning("CombineMultipleAudioClips: 클립 배열이 null이거나 비어있습니다.");
            return null;
        }

        // null이 아닌 클립들만 필터링
        List<AudioClip> validClips = new List<AudioClip>();
        foreach (var clip in clips)
        {
            if (clip != null)
                validClips.Add(clip);
        }

        if (validClips.Count == 0)
        {
            Debug.LogWarning("CombineMultipleAudioClips: 유효한 클립이 없습니다.");
            return null;
        }

        if (validClips.Count == 1)
            return validClips[0];

        // 첫 번째 클립부터 시작해서 순차적으로 합치기
        AudioClip result = validClips[0];
        for (int i = 1; i < validClips.Count; i++)
        {
            AudioClip temp = CombineAudioClips(result, validClips[i], $"{name}_step{i}");
            if (i > 1) // 중간 결과물은 메모리에서 해제 (첫 번째 원본 제외)
            {
                Object.DestroyImmediate(result);
            }
            result = temp;
        }

        return result;
    }
}

