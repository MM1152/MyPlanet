using UnityEngine;

public class HitParticle : MonoBehaviour
{
    private ParticleSystem particle;
    private float duration;

    public PoolsId PoolsId;
    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        duration = particle.main.duration;
    }

    private void Update()
    {
        duration -= Time.deltaTime;
        if(duration <= 0f)
        {
            if(gameObject.activeSelf)
                Managers.ObjectPoolManager.Despawn(PoolsId, gameObject);
        }
    }
}
