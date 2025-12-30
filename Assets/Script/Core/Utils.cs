using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Utils
{
    // private static Rect screenRect;
    // static Utils()
    // {
    //     var camera = Camera.main;
    //     var zDistance = Mathf.Abs(camera.transform.position.z);

    //     var bottomLeft = camera.ScreenToWorldPoint(new Vector3(0, 0, zDistance));
    //     var topRight = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, zDistance));

    //     screenRect = new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
    // }

    public static Rect GetScreenToWorldRect()
    {
        var camera = Camera.main;
        var zDistance = Mathf.Abs(camera.transform.position.z);

        var bottomLeft = camera.ScreenToWorldPoint(new Vector3(0, 0, zDistance));
        var topRight = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, zDistance));

        return new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
    }

    public static bool IsPointerOverUI(Vector2 screenPosition)
    {
        if (EventSystem.current == null) return false;

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

    // public static Rect GetScreenBounds()
    // {
    //     return screenRect;
    // }

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
    /// �� ���� AudioClip�� ���������� ���ļ� �ϳ��� AudioClip���� ����ϴ�.
    /// </summary>
    /// <param name="clip1">ù ��° ����� Ŭ��</param>
    /// <param name="clip2">�� ��° ����� Ŭ��</param>
    /// <param name="name">������ Ŭ���� �̸�</param>
    /// <returns>������ AudioClip</returns>
    public static AudioClip CombineAudioClips(AudioClip clip1, AudioClip clip2, string name = "CombinedClip")
    {
        if (clip1 == null || clip2 == null)
        {
            Debug.LogWarning("CombineAudioClips: �ϳ� �̻��� AudioClip�� null�Դϴ�.");
            return clip1 ?? clip2;
        }

        // �� Ŭ���� ���ļ��� �ٸ��� ���
        if (clip1.frequency != clip2.frequency)
        {
            Debug.LogWarning($"CombineAudioClips: Ŭ������ ���ļ��� �ٸ��ϴ�. ({clip1.frequency} vs {clip2.frequency})");
        }

        // �� Ŭ���� ä�� ���� �ٸ��� ���
        if (clip1.channels != clip2.channels)
        {
            Debug.LogWarning($"CombineAudioClips: Ŭ������ ä�� ���� �ٸ��ϴ�. ({clip1.channels} vs {clip2.channels})");
        }

        // �� ���� ���ļ��� ä�� ���� ���
        int frequency = Mathf.Max(clip1.frequency, clip2.frequency);
        int channels = Mathf.Max(clip1.channels, clip2.channels);

        // �� ���� �� ���
        int clip1Samples = clip1.samples;
        int clip2Samples = clip2.samples;
        int totalSamples = clip1Samples + clip2Samples;

        // ���ο� AudioClip ����
        AudioClip combinedClip = AudioClip.Create(name, totalSamples, channels, frequency, false);

        // ù ��° Ŭ�� ������ ��������
        float[] clip1Data = new float[clip1Samples * clip1.channels];
        clip1.GetData(clip1Data, 0);

        // �� ��° Ŭ�� ������ ��������
        float[] clip2Data = new float[clip2Samples * clip2.channels];
        clip2.GetData(clip2Data, 0);

        // ������ ������ �迭 ����
        float[] combinedData = new float[totalSamples * channels];

        // ù ��° Ŭ�� ������ ���� (ä�� �� ����)
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

        // �� ��° Ŭ�� ������ ���� (ä�� �� ����)
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

        // ������ �����͸� �� Ŭ���� ����
        combinedClip.SetData(combinedData, 0);

        return combinedClip;
    }

    /// <summary>
    /// ���� ���� AudioClip�� ���������� ���ļ� �ϳ��� AudioClip���� ����ϴ�.
    /// </summary>
    /// <param name="clips">��ĥ ����� Ŭ����</param>
    /// <param name="name">������ Ŭ���� �̸�</param>
    /// <returns>������ AudioClip</returns>
    public static AudioClip CombineMultipleAudioClips(AudioClip[] clips, string name = "CombinedClip")
    {
        if (clips == null || clips.Length == 0)
        {
            Debug.LogWarning("CombineMultipleAudioClips: Ŭ�� �迭�� null�̰ų� ����ֽ��ϴ�.");
            return null;
        }

        // null�� �ƴ� Ŭ���鸸 ���͸�
        List<AudioClip> validClips = new List<AudioClip>();
        foreach (var clip in clips)
        {
            if (clip != null)
                validClips.Add(clip);
        }

        if (validClips.Count == 0)
        {
            Debug.LogWarning("CombineMultipleAudioClips: ��ȿ�� Ŭ���� �����ϴ�.");
            return null;
        }

        if (validClips.Count == 1)
            return validClips[0];

        // ù ��° Ŭ������ �����ؼ� ���������� ��ġ��
        AudioClip result = validClips[0];
        for (int i = 1; i < validClips.Count; i++)
        {
            AudioClip temp = CombineAudioClips(result, validClips[i], $"{name}_step{i}");
            if (i > 1) // �߰� ������� �޸𸮿��� ���� (ù ��° ���� ����)
            {
                GameObject.DestroyImmediate(result);
            }
            result = temp;
        }

        return result;
    }

    public static DateTime CovertLongToServerTime(long timeStamp)
    {
        DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(timeStamp).UtcDateTime;
        return dateTime.AddHours(9);
    }
}

