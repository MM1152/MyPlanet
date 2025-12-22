using UnityEngine;

public class EnemyAsset : MonoBehaviour
{
    [SerializeField] private MeshRenderer enemyMeshRenderer;//교체할 머티리얼
    private float scale = 0.1f;
    private Vector3 baseRotation;
    private Enemy enemy;

    public void Init(Enemy enemy, Material material)
    {
        if (enemy == null) return;
        if (material == null) return;
        this.enemy = enemy;
        baseRotation = this.transform.rotation.eulerAngles;
        this.transform.SetParent(enemy.rotObj.transform, false);
        transform.localPosition = Vector3.zero;
        transform.rotation = enemy.rotObj.transform.rotation * Quaternion.Euler(-180, -90, 90);
        enemyMeshRenderer.sharedMaterial = material;
        
        if(enemy.enemyData.ID == 3057) //7스테이지보스가 유독큼
        {
            scale = 0.05f;
        }

        this.transform.localScale = Vector3.one * scale;
        enemy.enemyAsset = this;
        enemy.OnDie += RotReset;
    }

    private void RotReset(Enemy enemy)
    {
        this.transform.rotation = Quaternion.Euler(baseRotation);
        enemy.OnDie -= RotReset;
    }
}
