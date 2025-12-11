using UnityEngine;

public class ChaosBeacon : BaseAttackPrefab
{
    private float angle = 90f;
    private float duration;
    private TowerTable.UtilTower data;
    public override void Init(Tower data)
    {
        base.Init(data);
        poolsId = PoolsId.Repulsor;
        this.data = data.TowerData as TowerTable.UtilTower;
        duration = this.data.Duration;
        transform.localScale = new Vector3(this.data.range, this.data.range, this.data.range);
    }

    public void SetDir(Vector3 dir)
    {
        var angleRad = Mathf.Atan2(dir.y, dir.x);
        var angleDeg = angleRad * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angleDeg);
    }

    private void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0f)
        {
            if (gameObject.activeSelf)
                Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
        }
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagIds.EnemyTag))
        {
            var enemy = collision.GetComponent<Enemy>();
            if (EnemyTypes.IsEliteMonster(enemy.enemyData.ID) || EnemyTypes.IsBossMonster(enemy.enemyData.ID))
                return;

            var dir = (collision.transform.position - tower.TowerGameObject.transform.position).normalized;
            var angleRad = Mathf.Atan2(dir.y, dir.x);
            var angleDeg = angleRad * Mathf.Rad2Deg;
            float normalizedAngleDeg = (angleDeg + 360f) % 360f;

            bool isInAngle = Mathf.Abs(normalizedAngleDeg - transform.rotation.eulerAngles.z) < angle / 2f;

            if (isInAngle)
            {
                enemy.SetChaos(data.Duration);
            }
        }
    }

    public override void SetTarget(Transform target, float noise)
    {
        base.SetTarget(target, noise);
    }

    protected override void HitTarget(Collider2D collision)
    {
        throw new System.NotImplementedException();
    }
}