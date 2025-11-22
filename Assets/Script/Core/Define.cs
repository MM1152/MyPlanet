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
    TitleMainWindow = 2,
    TitleStageSelectedWindow = 3,
    TitlePresetWindow = 4,
    TitleTowerPlaceEditWindow = 5,
    TitleSelectPlanetWindow = 6,
}

public enum PopupIds
{
    None = -1,
    TowerInfomationPopup = 0
}

public static class SceneIds
{
    public static readonly string TitleScene = "TitleScene";
    public static readonly string GameScene = "GameScene";
    public static readonly string LoadingScene = "LoadingScene";
}

public enum ElementType
{
    None = -1,
    Normal = 0,
    Fire = 1,
    Steel = 2,
    Ice = 3,
    Light = 4,
    Dark = 5
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
    GravityControl = 207,
    ShockWaveBullet = 208,
    ShockWave = 209,
    
    SimpleBullet = 300,
    HomingBullet = 301,
    SpreadBullet = 302,
    Exp = 400,
    DamageText = 600,

    TestRange = 900,
}

public enum EnemyType
{
    Melee = 0,
    Ranged = 1,    
}

public static class AddressableNames
{
    private static readonly Dictionary<string, PoolsId> PoolsName = new()
    {
        { "Enemy", PoolsId.Enemy },
        { "SimpleBullet", PoolsId.SimpleBullet },
        { "HomingBullet", PoolsId.HomingBullet },
        { "SpreadBullet", PoolsId.SpreadBullet },
        { "Bullet", PoolsId.Bullet },
        { "Missile", PoolsId.Missile },
        { "SniperBullet", PoolsId.SniperBullet },
        { "Laser", PoolsId.Laser },
        { "Exp", PoolsId.Exp },
        { "MagmaBoomBullet", PoolsId.MagmaBoomBullet },
        { "FragmentBullet", PoolsId.FragmentBullet },
        { "Mine", PoolsId.Mine },
        { "GravityControl", PoolsId.GravityControl },
        { "ShockWaveBullet", PoolsId.ShockWaveBullet },
        { "ShockWave", PoolsId.ShockWave },
        { "DamageText", PoolsId.DamageText },
        { "TestRange", PoolsId.TestRange },
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
    public readonly static string TextUISpawnManagerTag = "TextUISpawnManager";
    public readonly static string PlayerTag = "Player";
    public readonly static string EnemyTag = "Enemy";
    public readonly static string EnemyProjectileTag = "EnemyProjectile";
}

public static class DataTableIds
{
    public static readonly string EnemyTable = "EnemyTable";
    public static readonly string CrewRankTable = "CrewRankTable";
    public static readonly string TowerTable = "TowerTable";
    public static readonly string WaveTable = "WaveTable";
    public static readonly string PresetTable = "PresetTable";
    public static readonly string PlanetTable = "PlanetTable";
    public static readonly string StringTable = "StringTable";
    public static readonly string PassiveTable = "PassiveTable";
    public static readonly string EffectTable = "EffectTable";

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

public static class DataBasePaths
{
    public static string UserPath = "/users/";
    public static string PresetPath = "/preset/";
}