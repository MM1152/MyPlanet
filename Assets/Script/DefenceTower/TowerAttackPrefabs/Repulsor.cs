using JetBrains.Annotations;
using UnityEngine;

public class Repulsor : BaseAttackPrefab
{
    private TowerTable.UtilTower data;
    private new EffectTable.Data effect;

    private float duration = 1f;
    public override void Init(Tower data)
    {
        base.Init(data);
        poolsId = PoolsId.Repulsor;
        this.data = data.TowerData as TowerTable.UtilTower;
        effect = this.data.Effect;
        duration = 1f;
        transform.localScale = new Vector3(data.BonusAttackRange , data.BonusAttackRange , data.BonusAttackRange);
    }

    public void SetDir(Vector3 dir)
    {
        var angleRad = Mathf.Atan2(dir.y, dir.x);
        var angleDeg = angleRad * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angleDeg);
    }

    protected override void HitTarget(Collider2D collision)
    {
        return;
    }

    private void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0f)
        {
            if(gameObject.activeSelf)
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
            bool isInAngle = Mathf.Abs(normalizedAngleDeg - transform.rotation.eulerAngles.z) < tower.BonuseNoise / 2f;

            if(isInAngle )
            {
                float force = effect.Val;
                enemy.PushEnemy(dir , force , 1f);
            }
        }
    }
}