using System.Runtime.CompilerServices;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class SolarLaser : BaseAttackPrefab
{
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private Vector2 towerSize;
    private BasePlanet basePlanet;
    private Vector2 baseScale;
    private float timer;

    private void Awake()
    {
        basePlanet = GameObject.FindWithTag(TagIds.PlayerTag).GetComponent<BasePlanet>();
        baseScale = transform.localScale;
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
        gameObject.transform.localScale = new Vector2(tower.BonusWidthSize * baseScale.x, tower.BonusAttackRange * baseScale.y);
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

    private void Update()
    {
        //transform.eulerAngles += Vector3.forward * rotationSpeed * Time.deltaTime;
        transform.eulerAngles += Vector3.forward * rotationSpeed * Time.deltaTime;
        var angle = transform.eulerAngles.z + 90f;
        transform.position = tower.TowerGameObject.transform.position + new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad) * towerSize.x * transform.localScale.y,
            Mathf.Sin(angle * Mathf.Deg2Rad) * towerSize.y * transform.localScale.y,
            0f);
        timer += Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(TagIds.EnemyTag))
        {
            if (timer < 60f / tower.FullAttackSpeed) return;

            timer = 0;
            var find = collision.GetComponent<IDamageAble>();

            var percent = tower.TypeEffectiveness.GetDamagePercent(find.ElementType);
            find.OnDamage((int)(tower.FullDamage * percent));
            basePlanet.PassiveSystem.CheckUseAblePassive(tower, null, collision.GetComponent<Enemy>());
        }
       
    }
}
