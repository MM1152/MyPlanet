using UnityEngine;

public class ProjectTile : MonoBehaviour
{
    [SerializeField] private Sprite sp;
    public float speed = 10f;
    public int damage = 10;
    protected Transform target;
    protected Vector3 dir;
    protected SpriteRenderer spriteRenderer;

    public virtual void Init(Transform target)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.target = target;
        dir = SetDir();

        float rad = Mathf.Atan2(dir.y , dir.x);
        transform.rotation = Quaternion.Euler(0f, 0f, rad * Mathf.Rad2Deg);

        if(sp != null) 
            spriteRenderer.sprite = sp;
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

        transform.position += dir * speed * Time.deltaTime;
    }

    protected virtual void HitTarget()
    {
        // 데미지 주는 로직 
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