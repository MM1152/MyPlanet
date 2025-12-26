using UnityEngine;

public class HitParticle : MonoBehaviour
{
    private ParticleSystem particle;
    private float duration;
    private Vector3 baseScale;
    public PoolsId PoolsId;
    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        baseScale = transform.localScale;
    }

    private void OnEnable()
    {
        duration = particle.main.duration;
    }

    private void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0f)
        {
            if (gameObject.activeSelf)
            {
                gameObject.transform.localScale = baseScale;    
                Managers.ObjectPoolManager.Despawn(PoolsId, gameObject);
            }
        }
    }
}
