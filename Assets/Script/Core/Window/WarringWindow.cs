using Cysharp.Threading.Tasks;
using System;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WarringWindow : Window
{
    [SerializeField] private TextMeshProUGUI warringText;
    [SerializeField] private CanvasGroup canvasGroup;

    public event Action closeEvent;
    public override void Init(WindowManager manager)
    {
        base.Init(manager);
        windowId = (int)WindowIds.WarringWindow;
    }

    public override void Open()
    {
        base.Open();
        Time.timeScale = 0f;
        canvasGroup.alpha = 1f;
        
        // 사이렌 사운드 재생 및 길이 기반 딜레이 설정
        float clipLength = Managers.SoundManager.GetClipLength(AudiosId.sci_fi_alarm_siren_loop_01);
        Managers.SoundManager.PlaySFX(AudiosId.sci_fi_alarm_siren_loop_01, 1f, true);
        WarringWindowDelayClose(clipLength, clipLength).Forget();
    }

    public override void Close()
    {
        base.Close();
    }

    public void SetWarringUI(EnemyType enemyType)
    {
        warringText.alpha = 1f;
        warringText.text = enemyType == EnemyType.EliteMonster ? $"<i>엘리트 몬스터 출현!<i>" : $"<i>보스 몬스터 출현!<i>";
    }

    private async UniTaskVoid WarringWindowDelayClose(float delay, float fadeDuration)
    {
        await UniTask.Delay((int)(delay * 2000), true, cancellationToken: this.GetCancellationTokenOnDestroy());

        float fadeSpeed = fadeDuration > 0f ? 1f / fadeDuration : 1f;
        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.unscaledDeltaTime * fadeSpeed;
            await UniTask.Yield();
        }
        manager.Close();
        Managers.SoundManager.StopSFX();
        closeEvent?.Invoke();
    }
}
