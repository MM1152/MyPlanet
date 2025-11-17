using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public enum WindowIds
{
    None = -1,
    PlaceTowerWindow = 0,
    StatusWindow = 1,
}

public enum PopupIds
{
    None = -1,
    TowerInfomationPopup = 0
}

public enum ElementType
{
    None = -1,
    Fire = 0,
    Steel = 1,
    Water = 2,
    Light = 3,
    Dark = 4
}

public static class Variable
{
    public static bool IsJoyStickActive;
}

public static class AddressableLabelIds
{
    public readonly static string PoolsIds = "Pools";
}

public enum PoolsId
{
    None = -1,

    Enemy = 100,

    Bullet = 200,
    Missile = 201,
    Laser = 202,
    SniperBullet = 203,
    MagmaBoomBullet = 204,
    FragmentBullet = 205,
    Mine = 206,

    Exp = 300,
}

public enum EnemyType
{
    Melee = 0,
    Ranged = 1,
}

public enum EnemyTier
{
    Tier3 = 3,
    Tier2 = 2,
    Tier1 = 1,
    Boss = 0,
}

public struct EnemyAttackKey
{
    //근접,원거리
    public EnemyType enemyType;
  
    public ElementType elementType; 
   
    public EnemyTier tier;
    public EnemyAttackKey(EnemyType enemyType, ElementType elementType, EnemyTier tier)
    {
        this.enemyType = enemyType;
        this.elementType = elementType;
        this.tier = tier;
    }   
}

public static class AddressableNames
{
    private static readonly Dictionary<string, PoolsId> PoolsName = new()
    {
        { "Enemy", PoolsId.Enemy },
        { "Bullet", PoolsId.Bullet },
        { "Missile", PoolsId.Missile },
        { "SniperBullet", PoolsId.SniperBullet },
        { "Laser", PoolsId.Laser },
        { "Exp", PoolsId.Exp },
        { "MagmaBoomBullet", PoolsId.MagmaBoomBullet },
        { "FragmentBullet", PoolsId.FragmentBullet },
        { "Mine", PoolsId.Mine },
    };

    public static PoolsId GetPoolsId(string name)
    {
        if (PoolsName.TryGetValue(name, out PoolsId id))
        {
            return id;
        }
        return PoolsId.None;
    }
}

public static class TagIds
{
    public readonly static string DefenseTowerTag = "DefenseTower";
    public readonly static string TowerManagerTag = "TowerManager";
    public readonly static string WindowManagerTag = "WindowManager";
    public readonly static string EnemySpawnManagerTag = "EnemySpawnManager";
    public readonly static string WaveManagerTag = "WaveManager";
}

public static class DataTableIds
{
    public static readonly string EnemyTable = "EnemyTable";
    public static readonly string CrewRankTable = "CrewRankTable";
    public static readonly string TowerTable = "TowerTable";
    public static readonly string WaveTable = "WaveTable";


    public static readonly HashSet<string> AllIds = new HashSet<string>()
    {
            EnemyTable
    };

    public static bool Contains(string id)
    {
        return AllIds.Contains(id);
    }

    public static IEnumerable<string> GetAllIds()
    {
        return AllIds;
    }

}