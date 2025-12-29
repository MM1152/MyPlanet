using UnityEngine;

public class AssetClockwiseRotator : EnemyAssetMove
{
    protected override float speed => 30f;
    public override void Move()
    {
        Debug.Log("[AssetClockwiseRotator] 에셋이 시계방향으로 회전합니다.");
        float angle = speed * Time.deltaTime;
        enemyAsset.transform.localRotation *= Quaternion.AngleAxis(-angle, Vector3.up);
        Debug.Log($"[AssetClockwiseRotator] 현재 각도: {enemyAsset.transform.eulerAngles.x}");
    }
}
