using System.Collections.Generic;
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
    DebugTowerPlaceWIndow = 7,
    TitleBookWindow = 8,
    TitleBookInfomationWindow = 9,
    VictoryWindow = 10,
    OptionUpgradeWindow = 11,
    TerraformingWindow = 12,
    WaveWindow = 13,
}

public enum PopupIds
{
    None = -1,
    TowerInfomationPopup = 0,
    UnLockPopup = 1,
}

public static class SceneIds
{
    public static readonly string TitleScene = "TitleScene";
    public static readonly string GameScene = "GameScene";
    public static readonly string LoadingScene = "LoadingScene";
    public static readonly string DebugModeScene = "DebugModeScene";
}

public enum ElementType
{
    None = -1,
    Normal = 0,
    Fire = 1,
    Ice = 2,
    Steel = 3,
    Light = 4,
    Dark = 5
}

public static class Variable
{
    public static bool IsJoyStickActive;
    public static bool IsTutorialActive;
    public static bool IsSpawnActive;
}

public static class AddressableLabelIds
{
    public readonly static string PoolsIds = "Pools";
    public readonly static string TypeIds = "Type";
    public readonly static string ElementTypeIds = "Element";
    public readonly static string AttackTypeIds = "AttackType";
}

public enum PoolsId
{
    None = -1,

    Enemy = 100,

    Bullet = 200,
    Missile = 201,
    SolarLaser = 202,
    SniperBullet = 203,
    MagmaBoomBullet = 204,
    FragmentBullet = 205,
    Mine = 206,
    GravityControl = 207,
    ShockWaveBullet = 208,
    ShockWave = 209,
    Explosion = 210,
    DarkLaser = 211,
    ShadowBursterBullet = 212,
    IceRangerMissile = 213,
    LuminaSniperBullet = 214,
    MagmaBoomFregment = 215,
    IronMine = 216,
    IronMineExplosion = 217,
    Surge = 218,
    BlackMineBullet = 219,
    ShadowSurge = 220,
    GravityWrap = 221,

    SimpleBullet = 300,
    HomingBullet = 301,
    SpreadBullet = 302,
    SwitchDirectionBullet = 303,
    RainBullet = 304,

    Exp = 400,
    DamageText = 600,

    TestRange = 900,
}

public enum EnemyType
{
    Melee = 0,
    Ranged = 1,
    EliteMonster = 2,
    Boss = 3,
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
        { "SolarLaser", PoolsId.SolarLaser },
        { "Exp", PoolsId.Exp },
        { "MagmaBoomBullet", PoolsId.MagmaBoomBullet },
        { "FragmentBullet", PoolsId.FragmentBullet },
        { "Mine", PoolsId.Mine },
        { "GravityControl", PoolsId.GravityControl },
        { "ShockWaveBullet", PoolsId.ShockWaveBullet },
        { "ShockWave", PoolsId.ShockWave },
        { "DamageText", PoolsId.DamageText },
        { "TestRange", PoolsId.TestRange },
        { "Explosion", PoolsId.Explosion },
        { "DarkLaser", PoolsId.DarkLaser },
        { "ShadowBursterBullet" , PoolsId.ShadowBursterBullet },
        { "IceRangerMissile" , PoolsId.IceRangerMissile },
        { "LuminaSniperBullet" , PoolsId.LuminaSniperBullet },
        { "MagmaBoomFregment" , PoolsId.MagmaBoomFregment },
        { "IronMine" , PoolsId.IronMine },
        { "IronMineExplosion" , PoolsId.IronMineExplosion },
        { "Surge" , PoolsId.Surge },
        { "BlackMineBullet" , PoolsId.BlackMineBullet },
        { "ShadowSurge" , PoolsId.ShadowSurge },
        { "GravityWrap" , PoolsId.GravityWrap },
        { "SwitchDirectionBullet", PoolsId.SwitchDirectionBullet },
        { "RainBullet", PoolsId.RainBullet },   
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
    public readonly static string IronMineTag = "IronMine";
    public readonly static string TutorialManagerTag = "TutorialManager"; 
    public readonly static string WaveWindowTag = "WaveWindow";
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
    public static readonly string LevelUpTable = "LevelUpTable";
    public static readonly string TypeSpriteTable = "TypeSpriteTable";
    public static readonly string ElementSpriteTable = "ElementSpriteTable";
    public static readonly string AttackTypeSpriteTable = "AttackTypeSpriteTable";
    public static readonly string OptionTable = "OptionTable";
    public static readonly string PlanetLevelUpTable = "PlanetLevelUpTable";
    public static readonly string TerraformingTable = "TerraformingTable";
    public static readonly string ConsumableTable = "ConsumableTable";

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
    public static string UserPath => "users/";
    public static string PresetPath => "preset/";
    public static string PlanetPath => "planet/";

    public static string GoldPath => UserPath + FirebaseManager.Instance.UserId + "/gold";
    public static string ExpPath => UserPath + FirebaseManager.Instance.UserId + "/exp";

    public static string PlanetDataPathFormating => PlanetPath + FirebaseManager.Instance.UserId + "/{0}";
    public static string PlanetLevelPathFormating => PlanetDataPathFormating + "/level";
    public static string PlanetPeiceCountPathFormating => PlanetDataPathFormating + "/count";
    public static string PlanetStarCountPathFormating => PlanetDataPathFormating + "/star";
    public static string PlanetOpenSlotPathFormating => PlanetDataPathFormating + "/openSlot";
}

public static class EnemyTypes
{
    private static readonly HashSet<int> BossMonsterIds = new HashSet<int> { 3027, 3032 };
    private static readonly HashSet<int> EliteMonseterIds = new HashSet<int> { 3026,3028,3029,3030,3031 };
    public static bool IsEliteMonster(int id) => EliteMonseterIds.Contains(id);
    public static bool IsBossMonster(int id) => BossMonsterIds.Contains(id);
}

public static class ColorDefine
{
    public static readonly Color TowerSelectUIColor = new Color(0.5058824f, 0.7921569f, 0.764706f, 1f);
    public static readonly Color ConsumableSelectUiColor = new Color(0.6235294f, 0.654902f, 0.8196079f, 1f);
}

public static class TerraformingData
{
    public enum TerraformingTargetType
    {
        Plant = 1,
        DefenseTower = 2,
        Tower = 3,
        Player = 4
    }

    public enum T_Effect_type
    {
        IncreaseGoldGain = 12014,
        IncreaseExpGain = 12015,
        IncreaseMaxHealth = 12017,
        IncreaseAttackSpeed = 12018,
        IncreaseAttackDamage = 12012,
        HealthRegeneration = 12019,
        IncreaseMovementSpeed = 12008,
        IncreaseDefense = 12016
    }

    public static HashSet<int> terraformingUnlockPoints = new HashSet<int>();
    public static int[] terrformingOpenValues = new int[4] { 20, 45, 75, 100 };
    private static readonly Dictionary<int, string> terraformingNameDataKeys = new()
    {
        { 6119 , "세이지 하버" },
        { 6120 , "로터스 필드" },
        { 6121 , "허브리움" },
        { 6122 , "솔라 루트" },
        { 6123 , "세렌시아" },
        { 6124 , "네오 시드"},
        { 6125 , "글로리페탈"},
        { 6126 , "엘레멘트 리프"}
    };

    private static readonly Dictionary<int, string> terraformingDescDataKeys = new()
    {
        { 6127 , "게임 종료 후 결산 시 얻는 골드 획득량이 5% 증가합니다." },
        { 6128 , "게임 종료 후 결산 시 얻는 경험치 획득량이 5% 증가합니다." },
        { 6129 , "행성의 최대 체력이 12% 증가합니다." },
        { 6130 , "방어 위성에 설치된 타워 전체의 공격 속도가 10% 증가합니다." },
        { 6131 , "행성의 공격력이 15% 증가합니다." },
        { 6132 , "1초마다 행성 체력이 1%씩 재생됩니다." },
        { 6133 , "방어 위성의 이동 속도가 25% 증가합니다." },
        { 6134 , "행성의 방어력이 15% 증가합니다." }
    };

    public static string GetTerraformingNameDataKey(int id)
    {
        if (terraformingNameDataKeys.TryGetValue(id, out string key))
        {
            return key;
        }
        return string.Empty;
    }

    public static string GetTerraformingDescriptionDataKey(int id)
    {
        if (terraformingDescDataKeys.TryGetValue(id, out string key))
        {
            return key;
        }
        return string.Empty;
    }
}

