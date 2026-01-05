using UnityEngine;
using System.Collections.Generic;
public class GravityWrap : BaseAttackPrefab
{
    protected string targetTag;
    protected Transform followTarget;
    protected float slowSpeedPercent;
    protected float duration;
    protected bool isElete;
    protected bool isDeleteProjectile;
    
    protected List<IMoveAble> moveAbles = new List<IMoveAble>();
    private UtilTower utiltower;
    private IFieldTower ownerTower;
    private float FullRange => utiltower.UtilTowerData?.range ?? 0 + tower.BonusAttackRange;
    [SerializeField] private GameObject[] filedParticle;

    public void SetOwnerTower(IFieldTower tower)
    {
        ownerTower = tower;
    }

    public override void Init(Tower data)
    {
        base.Init(data);

        for(int i = 0; i < filedParticle.Length; i++)
        {
            filedParticle[i].SetActive(false);
        }

        utiltower = data as UtilTower;
        poolsId = PoolsId.GravityWrap;
        transform.localScale = new Vector3(FullRange, FullRange, FullRange);
        duration = utiltower.FullDuration;
    }

    public void Setting(Transform followTarget , string targetTag , float slowSpeedPercent , bool isElete = false , bool isDeleteProjectile = false)
    {
        this.targetTag = targetTag;
        this.followTarget = followTarget;
        this.slowSpeedPercent = slowSpeedPercent;
        this.isElete = isElete;
        this.isDeleteProjectile = isDeleteProjectile;
    }

    /// <summary>
    /// 1 : �Ͼ� ���� , 2 : ������������ �ϴ� ��ƼŬ
    /// </summary>
    public void SetAssets(int particle)
    {
        for(int i = 0; i < filedParticle.Length; i++)
        {
            filedParticle[i].SetActive(false);
        }
        filedParticle[particle].SetActive(true);
    }
    
    protected override void HitTarget(Collider2D collision)
    {
        return;
    }

    protected void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0f)
        {
            if (gameObject.activeSelf)
                Managers.ObjectPoolManager.Despawn(poolsId, this.gameObject);
            return;
        }
        if(followTarget == null)
        {
            return;
        }
        transform.position = followTarget.position;
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(targetTag))
        {
            var enemy = collision.GetComponent<Enemy>();
            if(enemy != null)
            {
                bool isElete = EnemyTypes.IsEliteMonster(enemy.enemyData.ID);
                bool isBoss = EnemyTypes.IsBossMonster(enemy.enemyData.ID);
                if (isElete || isBoss)
                {
                    if (this.isElete)
                    {
                        var eleteMove = enemy as IMoveAble;
                        eleteMove.CurrentSpeed = eleteMove.BaseSpeed - (eleteMove.BaseSpeed * (slowSpeedPercent / 100f));
                        moveAbles.Add(eleteMove);
                    }
                    return;
                }
            }

            if (this.isElete)
                return;

            var moveAble = collision.GetComponent<IMoveAble>();
            if(isDeleteProjectile)
            {
                if(collision.gameObject.activeSelf)
                {
                    var enemyProjecttile = collision.GetComponent<EnemyProjectileBase>();
                    Managers.ObjectPoolManager.Despawn(enemyProjecttile.PoolsId, enemyProjecttile.gameObject);
                }
            }
            if(moveAble != null)
            {
                moveAble.CurrentSpeed = moveAble.BaseSpeed - (moveAble.BaseSpeed * (slowSpeedPercent / 100f));
                moveAbles.Add(moveAble);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            var moveAble = collision.GetComponent<IMoveAble>();
            if (moveAble != null)
            {
                moveAble.CurrentSpeed = moveAble.BaseSpeed;
                moveAbles.Remove(moveAble);
            }
        }
    }

    private void OnDisable()
    {
        foreach(var moveAble in moveAbles)
        {
            moveAble.CurrentSpeed = moveAble.BaseSpeed;
        }
        moveAbles.Clear();
        
        ownerTower?.ResetAttackCooldown();
        ownerTower = null;
    }
}
