using UnityEngine;

public class SolarLaser : BaseAttackPrefab
{
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private Vector2 towerSize;
    private BasePlanet basePlanet;
    private Vector2 baseScale;
    private float timer;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        basePlanet = GameObject.FindWithTag(TagIds.PlayerTag).GetComponent<BasePlanet>();
        baseScale = transform.localScale;
        lineRenderer = GetComponent<LineRenderer>();
    }

    public override void Init(Tower data)
    {
        base.Init(data);
        if(towerSize == Vector2.zero)
        {
            var collider = tower.TowerGameObject.GetComponent<BoxCollider2D>();
            towerSize = new Vector2(collider.size.x , collider.size.y);
        }
        timer = 0f;
    }

    public void UpgradeLaser()
    {
        if(!tower.UseAble)
        {
            gameObject.SetActive(false);
        }else
        {
            gameObject.SetActive(true);
        }
        var percentX = tower.BonusWidthSize * baseScale.x / baseScale.x;

        gameObject.transform.localScale = new Vector2(tower.BonusWidthSize * baseScale.x, tower.BonusAttackRange * baseScale.y);
        lineRenderer.startWidth = 0.5f * percentX;
    }

    public void UpdateLaser(float angle)
    {
        transform.eulerAngles = Vector3.forward * angle;
    }

    public override void SetTarget(Transform target, float noise)
    {
        base.SetTarget(target, noise);
    }

    protected override void HitTarget(Collider2D collision)
    {
        return;
    }

    private void FixedUpdate()
    {
        if (!tower.UseAble) return;
        //transform.eulerAngles += Vector3.forward * rotationSpeed * Time.deltaTime;
        transform.eulerAngles += Vector3.forward * rotationSpeed * Time.deltaTime;
        var angle = transform.eulerAngles.z + 90f;
        transform.position = tower.TowerGameObject.transform.position + new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad) * towerSize.x * transform.localScale.y,
            Mathf.Sin(angle * Mathf.Deg2Rad) * towerSize.y * transform.localScale.y,
            0f);


        // 0.3 �β��� linerederer �� 1 ������
        // 0.3 : 1 ����
        lineRenderer.SetPosition(0, tower.TowerGameObject.transform.position);
        lineRenderer.SetPosition(1, tower.TowerGameObject.transform.position + new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad) * towerSize.x * transform.localScale.y * 2f,
            Mathf.Sin(angle * Mathf.Deg2Rad) * towerSize.y * transform.localScale.y * 2f,
            0f));

        if (timer >= 60f / tower.FullAttackSpeed) timer = 0;
        timer += Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (timer < 60f / tower.FullAttackSpeed) return;

        if (collision.CompareTag(TagIds.EnemyTag))
        {
            var enemy = collision.GetComponent<Enemy>();
            if (enemy == null && collision.attachedRigidbody != null)
            {
                enemy = collision.attachedRigidbody.GetComponentInParent<Enemy>();
            }
            if (enemy == null) return;

            var barrier = enemy.GetComponentInChildren<Barrier>();
            if (barrier != null && !barrier.IsDead)
            {
                var percent = tower.TypeEffectiveness.GetDamagePercent(barrier.ElementType);
                Managers.SoundManager.PlaySFX(AudiosId.Flash_14);
                barrier.OnDamage((int)(tower.CalcurateAttackDamage * percent));
                var hitParticle = Managers.ObjectPoolManager.SpawnObject<HitParticle>(PoolsId.SolarLaserHitEffect);
                hitParticle.transform.position = collision.ClosestPoint(transform.position);
                return;
            }

            var find = enemy.GetComponent<IDamageAble>();
            if (find == null) return;
  
            var damagePercent = tower.TypeEffectiveness.GetDamagePercent(find.ElementType);
            Managers.SoundManager.PlaySFX(AudiosId.Flash_14);
            find.OnDamage((int)(tower.CalcurateAttackDamage * damagePercent));
            basePlanet.PassiveSystem.CheckUseAblePassive(tower, null, enemy);

            var hitParticle2 = Managers.ObjectPoolManager.SpawnObject<HitParticle>(PoolsId.SolarLaserHitEffect);
            hitParticle2.transform.position = collision.ClosestPoint(transform.position);
        }
       
    }

}
