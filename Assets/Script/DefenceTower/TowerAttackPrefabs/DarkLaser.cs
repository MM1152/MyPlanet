using UnityEngine;

public class DarkLaser : BaseAttackPrefab
{
    private BasePlanet basePlanet;
    private float attackIntervalTimer;
    private float timer;
    private Rect screenRect;
    private Vector2 baseScale;
    private LineRenderer lineRenderer;
    private void Awake()
    {
        basePlanet = GameObject.FindWithTag(TagIds.PlayerTag).GetComponent<BasePlanet>();
        screenRect = Screen.safeArea;
        baseScale = transform.localScale;
        lineRenderer = GetComponent<LineRenderer>();
    }

    public override void Init(Tower data)
    {
        base.Init(data);
        var startPoint = Camera.main.ScreenToWorldPoint(
            new Vector3(screenRect.xMin, Random.Range(screenRect.yMin, screenRect.yMax),
            -Camera.main.transform.position.z));
        // x ��ǥ�� safeArea�� �ִ�(x + width)�� ����
        var endPoint = Camera.main.ScreenToWorldPoint(
            new Vector3(screenRect.xMax, Random.Range(screenRect.yMin, screenRect.yMax),
            -Camera.main.transform.position.z));

        var plusDir = endPoint - startPoint;
        var reverseDir = startPoint - endPoint;
        Managers.SoundManager.PlaySFX(AudiosId.SFX_Spell01Layer01);
        lineRenderer.SetPosition(0, startPoint + reverseDir);
        lineRenderer.SetPosition(1, endPoint + plusDir);
        Managers.SoundManager.PlaySFX(AudiosId.SFX_Spell01Layer01);
        //var startPoint = Camera.main.ScreenToWorldPoint(new Vector3(screenRect.x, Random.Range(screenRect.y,screenRect.height) , -Camera.main.transform.position.z));
        //var endPoint = Camera.main.ScreenToWorldPoint(new Vector3(screenRect.width, Random.Range(screenRect.y, screenRect.height), -Camera.main.transform.position.z));

        transform.position = (startPoint + endPoint) / 2;
        //transform.position = new Vector3(0f, endPoint.y - startPoint.y, 0f);
        transform.eulerAngles = Vector3.forward * 90f;
        var dir = endPoint - startPoint;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation *= Quaternion.Euler(0f, 0f, angle);

        var percentX = tower.BonusWidthSize * baseScale.x / baseScale.x;

        lineRenderer.startWidth = 1 * percentX;
        transform.localScale = new Vector3(tower.BonusWidthSize * baseScale.x, baseScale.y, 1f);
        poolsId = PoolsId.DarkLaser;
    }

    public override void SetTarget(Transform target, float noise)
    {
        base.SetTarget(target, noise);
    }

    private void FixedUpdate()
    {
        if (attackIntervalTimer >= 60f / tower.FullAttackSpeed)
        {
            attackIntervalTimer = 0;
        }

        attackIntervalTimer += Time.deltaTime;

        timer += Time.deltaTime;
        if (timer >= tower.BonusDuration)
        {
            timer = 0;
            if (gameObject.activeSelf)
                Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(TagIds.EnemyTag))
        {
            if (attackIntervalTimer > 60f / tower.FullAttackSpeed) return;

            var find = collision.GetComponent<IDamageAble>();
            if (find != null)
            {
                var percent = tower.TypeEffectiveness.GetDamagePercent(find.ElementType);
                find.OnDamage((int)(tower.CalcurateAttackDamage * percent));
                basePlanet.PassiveSystem.CheckUseAblePassive(tower, null, collision.GetComponent<Enemy>());

                var particle = Managers.ObjectPoolManager.SpawnObject<HitParticle>(PoolsId.DarkLaserHitEffect);
                particle.transform.position = collision.ClosestPoint(transform.position);
            }
        }
    }

    protected override void HitTarget(Collider2D collision)
    {
        return;
    }
}