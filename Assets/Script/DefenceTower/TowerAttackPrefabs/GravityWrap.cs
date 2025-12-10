using UnityEngine;
using System.Collections.Generic;
public class GravityWrap : BaseAttackPrefab
{
    protected string targetTag;
    protected Transform followTarget;
    protected float slowSpeedPercent;

    protected float duration;
    

    protected List<IMoveAble> moveAbles = new List<IMoveAble>();
    private UtilTower utiltower;
    public override void Init(Tower data)
    {
        base.Init(data);

        utiltower = data as UtilTower;
        poolsId = PoolsId.GravityWrap;
        transform.localScale = new Vector3(utiltower.UtilTowerData.range, utiltower.UtilTowerData.range, utiltower.UtilTowerData.range);
        duration = tower.BonusDuration;
    }

    public void Setting(Transform followTarget , string targetTag , float slowSpeedPercent)
    {
        this.targetTag = targetTag;
        this.followTarget = followTarget;
        this.slowSpeedPercent = slowSpeedPercent;
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
        transform.position = followTarget.position;
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(targetTag))
        {
            var moveAble = collision.GetComponent<IMoveAble>();
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
    }
}
