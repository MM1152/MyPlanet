using UnityEngine;

public class ProjectTile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    protected Transform target;
    protected Vector3 dir;

    public virtual void Init(Transform target)
    {
        this.target = target;
        dir = SetDir();

        float rad = Mathf.Atan2(dir.y , dir.x);
        transform.rotation = Quaternion.Euler(0f, 0f, rad * Mathf.Rad2Deg);
    }

    protected virtual Vector3 SetDir()
    {
        dir = target.position - transform.position;
        return dir.normalized;
    }
    
    protected virtual void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.localPosition += dir * speed * Time.deltaTime;
    }

    private void HitTarget()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            IDamageAble damager = collision.GetComponent<IDamageAble>();
            if (damager != null)
            {
                damager.OnDamage(damage);
                HitTarget();
            }
        }
    }
}