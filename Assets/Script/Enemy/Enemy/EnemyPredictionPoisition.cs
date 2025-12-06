using JetBrains.Annotations;
using System;
using UnityEngine;

[Serializable]
public class EnemyPredictionPoisition
{
    private Enemy enemy;

    public void Init(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public Vector3 GetPredictionPosition(float predictionLimitTime)
    {
        return enemy.transform.position + ((Vector3)enemy.move.Direction * enemy.CurrentSpeed * predictionLimitTime);
    }
}
