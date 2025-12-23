using Cysharp.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WarringWindow : Window
{
    [SerializeField] private TextMeshProUGUI warringText;
    [SerializeField] private CanvasGroup canvasGroup;

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
        WarringWindowDelayClose(1f).Forget();
    }

    public override void Close()
    {
        base.Close();
    }

    public void SetWarringUI(EnemyType enemyType)
    {
        warringText.alpha = 1f;
        warringText.text = enemyType == EnemyType.EliteMonster ? $"<i>엘리트 보스 몬스터 출현!<i>" : $"<i>보스 몬스터 출현!<i>";
    }

    private async UniTaskVoid WarringWindowDelayClose(float delay = 1f)
    {
        await UniTask.Delay((int)(delay * 1000), true, cancellationToken: this.GetCancellationTokenOnDestroy());

        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.unscaledDeltaTime;
            await UniTask.Yield();
        }
        manager.Close();
    }





}
