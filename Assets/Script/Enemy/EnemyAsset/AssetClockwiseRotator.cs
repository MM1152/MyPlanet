using UnityEngine;

public class AssetClockwiseRotator : EnemyAssetMove
{
    protected override float speed => 30f;
    public override void Move()
    {
        float angle = speed * Time.deltaTime;
        enemyAsset.transform.localRotation *= Quaternion.AngleAxis(-angle, Vector3.up);
    }
}
