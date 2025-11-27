using UnityEngine;
using UnityEngine.UIElements;

public class ShadowSurge : BaseAttackPrefab
{
    [SerializeField] private GameObject pillar;
    [SerializeField] private GameObject attackRange;
    private Rect screenRect;
    private float duration = 0.1f;
    private Renderer render;

    private void Awake()
    {
        render = pillar.GetComponent<Renderer>();    
    }

    public override void Init(Tower data)
    {
        base.Init(data);
        screenRect = Screen.safeArea;

        var xPos = Random.Range(screenRect.xMin, screenRect.xMax);
        var yPos = Random.Range(screenRect.yMin, screenRect.yMax);

        // 월드 좌표 계산
        Vector3 bottomPos = Camera.main.ScreenToWorldPoint(new Vector3(xPos, yPos, -Camera.main.transform.position.z));
        Vector3 topPos = Camera.main.ScreenToWorldPoint(new Vector3(xPos, screenRect.yMax, -Camera.main.transform.position.z));

        float requiredHeight = topPos.y - bottomPos.y;
        float originalHeight = render.bounds.size.y / pillar.transform.localScale.y;
        float scaleMultiplier = requiredHeight / originalHeight;

        transform.position = bottomPos;

        Vector3 currentScale = pillar.transform.localScale;
        pillar.transform.localScale = new Vector3(currentScale.x, scaleMultiplier, currentScale.z);

        pillar.transform.position = Vector3.Lerp(bottomPos, topPos, 0.5f);

        duration = 0.2f;
    }

    private void Update()
    {
        duration -= Time.deltaTime;
        if(duration <= 0f)
        {
            Managers.ObjectPoolManager.Despawn(PoolsId.ShadowSurge , gameObject);
        }

    }

    public override void SetTarget(Transform target, float noise)
    {
        base.SetTarget(target, noise);
    }

    protected override void HitTarget(Collider2D collision)
    {
        var find = collision.GetComponent<IDamageAble>();
        if(find != null)
        {
            var percent = tower.TypeEffectiveness.GetDamagePercent(find.ElementType);
            find.OnDamage((int)(tower.CalcurateAttackDamage * percent));
        }
    }
}