using System;
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
    WarringWindow = 14,
    PauseWindow = 15,
    TitleShopWindow = 16,
    TowerInfomationWindow = 17,
    RandomPickUpWindow = 18,
    TitlePickUpResultWindow = 19,
    TitleStackRewardPlayWindow = 20,
}

public enum PopupIds
{
    None = -1,
    TowerInfomationPopup = 0,
    UnLockPopup = 1,
    PlayGamePopup = 2,
    SideBarPopup = 3,
    TextPopup = 4,
    RandomOptionPopup = 5,
    PurchaseTowerPopup = 6,
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
    public static bool IsDebugMode;
}

public static class AddressableLabelIds
{
    public readonly static string PoolsIds = "Pools";
    public readonly static string TypeIds = "Type";
    public readonly static string ElementTypeIds = "Element";
    public readonly static string AttackTypeIds = "AttackType";
    public readonly static string AudiosIds = "Audios";
}

public static class AddressableFormatPaths
{
    public readonly static string PlanetPrefabFormating = "Assets/DAX/Low Poly Solar System/Prefabs/{0}.prefab";
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
    PlutoBullet = 222,
    Drone = 223,
    Repulsor = 224,
    ChaosBeacon = 225,
    HellFireBullet = 226,
    FrostRepeaterBullet = 227,
    BurstBlasterBullet = 228,
    ShadowBursterFragment = 229,
    IceRangerFregment = 230,
    FrostBoomerBullet = 231,

    SimpleBullet = 300,
    HomingBullet = 301,
    SpreadBullet = 302,
    SwitchDirectionBullet = 303,
    RainBullet = 304,
    ArcMissileBullet = 305,
    LaserBeam4RedFlash = 306,
    LaserBeam4RedHit = 307,


    IceBarrier = 502,
    SteelBarrier = 503,
    Exp = 400,
    Zone = 500,
    DamageText = 600,

    TestRange = 900,

    SolarLaserHitEffect = 1000,
    DarkLaserHitEffect = 1001,
    Flash13redlaser = 1002,
    Hit13redlaser = 1003,
    Projectile13redlaser = 1004,
    Flash6bluefire = 1005,
    Hit6bluefire = 1006,
    Projectile6bluefire = 1007,
    Hit8dagger = 1008,
    Projectile8dagger = 1009,
    Hit23cube = 1010,
    Flash23cube = 1011,
    Projectile7pink = 1012,
    Flash7pink = 1013,
    Hit7pink = 1014,
    Flash26bluecrystal = 1015,
    Hit26bluecrystal = 1016,
    Hit10bluelaser = 1017,
    Flash10bluelaser = 1018,
    Flash18novaorange = 1019,
    Hit18novaorange = 1020,
    Flash23cube1 = 1021,
    Hit23cube1 = 1022,
    Flash2electro = 1023,
    Flash17novaviolet = 1024,
    Hit17novaviolet = 1025,
    Hit20pinkarrow = 1026,
    Flash19circlebomb = 1027,
    Hit19circlebomb = 1028,
    HitExplosionParticle = 1029,
    HitHealParticle = 1030,


    Melee_Tier03 = 1200,
    Melee_Tier02 = 1201,
    Melee_Tier01 = 1202,
    Ranged_Tier02 = 1300,
    Ranged_Tier01 = 1301,
    EliteMonster_Fire = 1400,
    EliteMonster_Ice = 1401,
    EliteMonster_Steel = 1402,
    EliteMonster_Light = 1403,
    EliteMonster_Dark = 1404,
    StageBoss_01 = 1500,
    StageBoss_02 = 1501,
    StageBoss_03 = 1502,
    StageBoss_04 = 1503,
    StageBoss_05 = 1504,
    StageBoss_06 = 1505,
    StageBoss_07 = 1506,
    StageBoss_08 = 1507,
    StageBoss_09 = 1508,
    StageBoss_10 = 1509,

    BossPartner = 1600,
}

public enum AudiosId
{
    None = -1,
    //BGM 
    Universe_Music = 0,
    Simple_Acoustics = 1,
    Battle_Music = 2,
    A_Dope_Chill_Session = 3,
    Dry_To_Verb = 4,
    //SFX
    ui_button_simple_click_03 = 100,
    ui_button_simple_click_05 = 101,
    ui_button_simple_click_06 = 102,
    ui_button_simple_click_07 = 103,
    ui_menu_button_beep_12 = 104,
    ui_menu_button_beep_13 = 105,
    ui_menu_button_beep_16 = 106,
    ui_menu_button_beep_17 = 107,
    ui_menu_button_scroll_05 = 108,
    ui_menu_button_scroll_page_03 = 109,
    ui_menu_button_scroll_back_05 = 110,
    ui_menu_button_confirm_03 = 111,
    collect_item_03 = 112,
    collect_item_05 = 113,
    collect_item_11 = 114,
    ui_menu_popup_message_04 = 115,
    ui_menu_popup_message_reward_01 = 116,
    magic_pop_open_01 = 117,
    magic_pop_open_03 = 118,
    cash_register_open_coins_cha_ching_01 = 119,
    jingle_chime_04_positive = 120,
    jingle_chime_22_negative = 121,
    //Attack SFX
    Flash_3 = 200,
    Flash_14 = 201,
    Hit_7 = 202,
    Hit_8 = 203,
    Hit_10 = 204,
    sci_fi_weapon_laser_small_02 = 205,
    SFX_Spell01Cast01 = 206,
    SFX_Spell01CastLayer01 = 207,
    SFX_Spell01CastLayer03 = 208,
    SFX_Spell01Dark = 209,
    SFX_Spell01Layer01 = 210,
    SFX_Spell01Layer04 = 211,
    SFX_Spell02Thunder = 212,
    //item SFX
    jw3_swoosh_rod_pole_007 = 300,
    magic_flame_of_light_03 = 301,
    magic_light_bubble_01 = 302,
    retro_magic_spell_cast_04 = 303,

    sci_fi_alarm_siren_loop_01 = 400,
}

public enum EnemyType
{
    None = -1,
    Melee = 0,
    Ranged = 1,
    EliteMonster = 2,
    Boss = 3,
}

public enum SoundType
{
    None = -1,
    BGM,
    SFX
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
        { "ArcMissileBullet", PoolsId.ArcMissileBullet },
        { "RainBullet", PoolsId.RainBullet },
        { "PlutoBullet", PoolsId.PlutoBullet },
        { "Drone", PoolsId.Drone },
        { "Repulsor", PoolsId.Repulsor },
        { "ChaosBeacon", PoolsId.ChaosBeacon },
        { "IceBarrier", PoolsId.IceBarrier },
        {"SteelBarrier", PoolsId.SteelBarrier },
        { "DarkLaserHitEffect", PoolsId.DarkLaserHitEffect },
        { "SolarLaserHitEffect", PoolsId.SolarLaserHitEffect },
        { "Flash 13 red laser", PoolsId.Flash13redlaser },
        { "Hit 13 red laser", PoolsId.Hit13redlaser },
        { "2D Projectile 13 red laser", PoolsId.Projectile13redlaser },
        { "HellFireBullet", PoolsId.HellFireBullet },
        { "Flash 6 blue fire", PoolsId.Flash6bluefire },
        { "Hit 6 blue fire", PoolsId.Hit6bluefire },
        { "2D Projectile 6 blue fire", PoolsId.Projectile6bluefire },
        { "FrostRepeaterBullet", PoolsId.FrostRepeaterBullet },
        { "Hit 8 dagger", PoolsId.Hit8dagger },
        { "2D Projectile 8 dagger", PoolsId.Projectile8dagger },
        { "BurstBlasterBullet", PoolsId.BurstBlasterBullet },
        { "Hit 23 cube", PoolsId.Hit23cube },
        { "Flash 23 cube", PoolsId.Flash23cube },
        { "ShadowBursterFragment", PoolsId.ShadowBursterFragment },
        { "2D Projectile 7 pink", PoolsId.Projectile7pink },
        { "Flash 7 pink", PoolsId.Flash7pink },
        { "Hit 7 pink", PoolsId.Hit7pink },
        { "Flash 26 blue crystal", PoolsId.Flash26bluecrystal },
        { "Hit 26 blue crystal", PoolsId.Hit26bluecrystal },
        { "IceRangerFregment", PoolsId.IceRangerFregment },
        { "Flash 10 blue laser", PoolsId.Flash10bluelaser },
        { "Hit 10 blue laser", PoolsId.Hit10bluelaser },
        { "Flash 18 nova orange", PoolsId.Flash18novaorange },
        { "Hit 18 nova orange", PoolsId.Hit18novaorange },
        { "Hit 23 cube 1", PoolsId.Hit23cube1 },
        { "Flash 23 cube 1", PoolsId.Flash23cube1 },
        { "FrostBoomerBullet", PoolsId.FrostBoomerBullet },
        { "Flash 2 electro", PoolsId.Flash2electro },
        { "Flash 17 nova violet", PoolsId.Flash17novaviolet },
        { "Hit 17 nova violet", PoolsId.Hit17novaviolet },
        { "Hit 20 pink arrow", PoolsId.Hit20pinkarrow },
        { "Flash 19 circle bomb", PoolsId.Flash19circlebomb },
        { "Hit 19 circle bomb", PoolsId.Hit19circlebomb },
        { "Laser beam 4 red Flash", PoolsId.LaserBeam4RedFlash },
        { "Laser beam 4 red Hit", PoolsId.LaserBeam4RedHit },
        { "Melee_Tier03", PoolsId.Melee_Tier03 },
        { "Melee_Tier02", PoolsId.Melee_Tier02 },
        { "Melee_Tier01", PoolsId.Melee_Tier01 },   
        { "Ranged_Tier02", PoolsId.Ranged_Tier02 },
        { "Ranged_Tier01", PoolsId.Ranged_Tier01 },
        { "EliteMonster_Fire", PoolsId.EliteMonster_Fire },
        { "EliteMonster_Ice", PoolsId.EliteMonster_Ice },
        { "EliteMonster_Steel", PoolsId.EliteMonster_Steel },
        { "EliteMonster_Light", PoolsId.EliteMonster_Light },
        { "EliteMonster_Dark", PoolsId.EliteMonster_Dark },
        { "StageBoss_01", PoolsId.StageBoss_01 },
        { "StageBoss_02", PoolsId.StageBoss_02 },
        { "StageBoss_03", PoolsId.StageBoss_03 },
        { "StageBoss_04", PoolsId.StageBoss_04 },
        { "StageBoss_05", PoolsId.StageBoss_05 },
        { "StageBoss_06", PoolsId.StageBoss_06 },
        { "StageBoss_07", PoolsId.StageBoss_07 },
        { "StageBoss_08", PoolsId.StageBoss_08 },
        { "StageBoss_09", PoolsId.StageBoss_09 },
        { "StageBoss_10", PoolsId.StageBoss_10 },
        { "BossPartner", PoolsId.BossPartner },
        {"HitExplosionParticle",PoolsId.HitExplosionParticle},
        {"HitHealParticle",PoolsId.HitHealParticle},
        {"Zone", PoolsId.Zone },    
    };

    private static readonly Dictionary<string, AudiosId> AudiosName = new()
    {
        {"Universe Music", AudiosId.Universe_Music },
        {"Simple Acoustics", AudiosId.Simple_Acoustics },
        {"Battle Music", AudiosId.Battle_Music },
        {"A Dope Chill Session", AudiosId.A_Dope_Chill_Session },
        {"Dry To Verb", AudiosId.Dry_To_Verb },
        {"ui_button_simple_click_03", AudiosId.ui_button_simple_click_03 },
        {"ui_button_simple_click_05", AudiosId.ui_button_simple_click_05 },
        {"ui_button_simple_click_06", AudiosId.ui_button_simple_click_06 },
        {"ui_button_simple_click_07", AudiosId.ui_button_simple_click_07 },
        { "ui_menu_button_beep_12", AudiosId.ui_menu_button_beep_12 },
        {"ui_menu_button_beep_13", AudiosId.ui_menu_button_beep_13 },
        {"ui_menu_button_beep_16", AudiosId.ui_menu_button_beep_16 },
        {"ui_menu_button_beep_17", AudiosId.ui_menu_button_beep_17 },
        {"ui_menu_button_scroll_05", AudiosId.ui_menu_button_scroll_05 },
        {"ui_menu_button_scroll_page_03", AudiosId.ui_menu_button_scroll_page_03 },
        {"ui_menu_button_scroll_back_05", AudiosId.ui_menu_button_scroll_back_05 },
        {"ui_menu_button_confirm_03", AudiosId.ui_menu_button_confirm_03 },
        {"collect_item_03", AudiosId.collect_item_03 },
        {"collect_item_05", AudiosId.collect_item_05 },
        {"collect_item_11", AudiosId.collect_item_11 },
        {"ui_menu_popup_message_04", AudiosId.ui_menu_popup_message_04 },
        {"ui_menu_popup_message_reward_01", AudiosId.ui_menu_popup_message_reward_01 },
        {"magic_pop_open_01", AudiosId.magic_pop_open_01 },
        {"magic_pop_open_03", AudiosId.magic_pop_open_03 },
        {"cash_register_open_coins_cha_ching_01", AudiosId.cash_register_open_coins_cha_ching_01 },
        {"jingle_chime_04_positive", AudiosId.jingle_chime_04_positive },
        {"jingle_chime_22_negative", AudiosId.jingle_chime_22_negative },
        {"Flash 3", AudiosId.Flash_3 },
        {"Flash 14", AudiosId.Flash_14 },
        {"Hit 7", AudiosId.Hit_7 },
        {"Hit 8", AudiosId.Hit_8 },
        {"Hit 10", AudiosId.Hit_10 },
        {"sci-fi_weapon_laser_small_02", AudiosId.sci_fi_weapon_laser_small_02 },
        {"SFX_Spell01Cast01", AudiosId.SFX_Spell01Cast01 },
        {"SFX_Spell01CastLayer01", AudiosId.SFX_Spell01CastLayer01 },
        {"SFX_Spell01CastLayer03", AudiosId.SFX_Spell01CastLayer03 },
        {"SFX_Spell01Dark", AudiosId.SFX_Spell01Dark },
        {"SFX_Spell01Layer01", AudiosId.SFX_Spell01Layer01 },
        {"SFX_Spell01Layer04", AudiosId.SFX_Spell01Layer04 },
        {"SFX_Spell02Thunder", AudiosId.SFX_Spell02Thunder },
        {"jw3_swoosh_rod_pole-007", AudiosId.jw3_swoosh_rod_pole_007 },
        {"magic_flame_of_light_03", AudiosId.magic_flame_of_light_03 },
        {"magic_light_bubble_01", AudiosId.magic_light_bubble_01 },
        {"retro_magic_spell_cast_04", AudiosId.retro_magic_spell_cast_04 },
        {"sci-fi_alarm_siren_loop_01", AudiosId.sci_fi_alarm_siren_loop_01 },
    };

    public static PoolsId GetPoolsId(string name)
    {
        if (PoolsName.TryGetValue(name, out PoolsId id))
        {
            return id;
        }
        return PoolsId.None;
    }
    public static AudiosId GetAudiosId(string name)
    {
        if (AudiosName.TryGetValue(name, out AudiosId id))
        {
            return id;
        }
        return AudiosId.None;
    }
}

public static class TagIds
{
    public readonly static string None = "Untagged";
    public readonly static string TerraFormingHighlightImage = "TerraFormingImage";
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
    public readonly static string DroneTag = "Drone";
    public readonly static string EnemyAssetManagerTag = "EnemyAssetManager";
    public readonly static string TutorialTaget = "TutorialTarget";
    public readonly static string BackButton = "BackButton";
    public readonly static string MainGameSceneCanvas = "GameSceneCanvas";
    public readonly static string PopupManager = "PopupManager";
}

public static class DataTableIds
{
    public static readonly string EnemyTable = "EnemyTable";
    public static readonly string SoundTable = "SoundsTable";
    public static readonly string CrewRankTable = "CrewRankTable";
    public static readonly string TowerTable = "TowerTable";
    public static readonly string UtilTowerTable = "UtilTowerTable";
    public static readonly string WaveTable = "WaveTable";
    public static readonly string StageTable = "StageTable";
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
    public static readonly string BasePlanetLevelTable = "BasePlanetLevelTable";
    public static readonly string ShopTable = "ShopTable";
    public static readonly string PlanetPassiveLevelUpTable = "PlanetPassiveLevelUpTable";
    public static readonly string StageInfomationTable = "StageInfomationTable";
    public static readonly string RandomPickUpTable = "RandomPickUpTable";
    public static readonly string ItemTable = "ItemTable";
    public static readonly string TowerRandomOptionValueTable = "TowerRandomOptionValueTable";
    public static readonly string TowerRandomOptionValuePercentTable = "TowerRandomOptionValuePercentTable";
    public static readonly string TowerDuplicationRewardTable = "TowerDuplicationRewardTable";
    public static readonly string RewardListTable = "RewardListTable";
    public static readonly string ClearWaveRewardTable = "ClearWaveRewardTable";

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
    public static string TowerPath => "tower/";
    public static string AsyncPlayerPath => "asyncPlayers/";

    public static string GoldPath => UserPath + FirebaseManager.Instance.UserId + "/gold";
    public static string ExpPath => UserPath + FirebaseManager.Instance.UserId + "/exp";
    public static string DiamondPath => UserPath + FirebaseManager.Instance.UserId + "/diamond";

    public static string PlanetDataPathFormating => PlanetPath + FirebaseManager.Instance.UserId + "/{0}";
    public static string PlanetLevelPathFormating => PlanetDataPathFormating + "/level";
    public static string PlanetPeiceCountPathFormating => PlanetDataPathFormating + "/count";
    public static string PlanetStarCountPathFormating => PlanetDataPathFormating + "/star";
    public static string PlanetOpenSlotPathFormating => PlanetDataPathFormating + "/openSlot";
    public static string PlanetUnlockPathFormating => PlanetDataPathFormating + "/unlocked";

    public static string TowerDataPathFormating => TowerPath + FirebaseManager.Instance.UserId + "/{0}";
    public static string TowerUnlockPathFormating => TowerDataPathFormating + "/Unlock";

    public static string AsyncStagePathFormating => AsyncPlayerPath + "stage{0}/";
    public static string AsyncPlayerSavePathFormating => AsyncStagePathFormating + FirebaseManager.Instance.UserId;
}

public static class EnemyTypes
{
    private static readonly HashSet<int> BossMonsterIds = new HashSet<int> { 3027, 3032, 3037, 3042, 3047, 3052, 3057, 3062, 3067, 3072 };
    private static readonly HashSet<int> EliteMonseterIds = new HashSet<int>
    {
         3026, 3028, 3029, 3030, 3031, 3033,3034, 3035, 3036, 3038, 3039, 3040, 3041, 3043, 3044, 3045, 3046, 3048, 3049, 3050, 3051, 3053, 3054, 3055, 3056, 3058, 3059,
          3060, 3061, 3063, 3064, 3065, 3066, 3068, 3069, 3070, 3071,};
    public static bool IsEliteMonster(int id) => EliteMonseterIds.Contains(id);
    public static bool IsBossMonster(int id) => BossMonsterIds.Contains(id);
}

public static class EnemyTier
{
    private static readonly Dictionary<int, int> monsterTierMap = new()
    {
        { 3001, 3 }, { 3002, 3 }, { 3003, 3 }, { 3004, 3 }, { 3005, 3 },
        { 3006, 2 }, { 3007, 2 }, { 3008, 2 }, { 3009, 2 }, { 3010, 2 },
        { 3011, 2 }, { 3012, 2 }, { 3013, 2 }, { 3014, 2 }, { 3015, 2 },
        { 3016, 1 }, { 3017, 1 }, { 3018, 1 }, { 3019, 1 }, { 3020, 1 },
        { 3021, 1 }, { 3022, 1 }, { 3023, 1 }, { 3024, 1 }, { 3025, 1 },
    };

    public static int GetTier(int id)
    {
        return monsterTierMap.TryGetValue(id, out var tier) ? tier : 0;
    }
}

public static class MonsterTierAssetMap
{
    public static readonly Dictionary<(EnemyType, int), PoolsId> monsterTierAssetMap = new()
    {
        { (EnemyType.Melee, 1), PoolsId.Melee_Tier01 },
        { (EnemyType.Melee, 2), PoolsId.Melee_Tier02 },
        { (EnemyType.Melee, 3), PoolsId.Melee_Tier03 },
        { (EnemyType.Ranged, 1), PoolsId.Ranged_Tier01 },
        { (EnemyType.Ranged, 2), PoolsId.Ranged_Tier02 },
    };

    public static PoolsId GetMonsterTierAsset(EnemyType enemyType, int tier)
    {
        if (monsterTierAssetMap.TryGetValue((enemyType, tier), out PoolsId poolId))
        {
            return poolId;
        }
        return PoolsId.None;
    }
}

public static class EliteMonsterAssetMap
{
    public static readonly Dictionary<ElementType, PoolsId> eliteMonsterAssetMap = new()
    {
        { ElementType.Fire, PoolsId.EliteMonster_Fire },
        { ElementType.Ice, PoolsId.EliteMonster_Ice },
        { ElementType.Steel, PoolsId.EliteMonster_Steel },
        { ElementType.Light, PoolsId.EliteMonster_Light },
        { ElementType.Dark, PoolsId.EliteMonster_Dark },
    };

    public static PoolsId GetEliteMonsterAsset(ElementType elementType)
    {
        if (eliteMonsterAssetMap.TryGetValue(elementType, out PoolsId poolId))
        {
            return poolId;
        }
        return PoolsId.None;
    }
}

public static class BossAssetMap
{
    public static readonly Dictionary<int, PoolsId> bossAssetMap = new()
    {
        { 3027, PoolsId.StageBoss_01 },
        { 3032, PoolsId.StageBoss_02 },
        { 3037, PoolsId.StageBoss_03 },
        { 3042, PoolsId.StageBoss_04 },
        { 3047, PoolsId.StageBoss_05 },
        { 3052, PoolsId.StageBoss_06 },
        { 3057, PoolsId.StageBoss_07 },
        { 3062, PoolsId.StageBoss_08 },
        { 3067, PoolsId.StageBoss_09 },
        { 3072, PoolsId.StageBoss_10 },
    };

    public static PoolsId GetBossAsset(int id)
    {
        if (bossAssetMap.TryGetValue(id, out PoolsId poolId))
        {
            return poolId;
        }
        return PoolsId.None;
    }
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
public static class LevelUpEffectDescriptions
{
    public static Dictionary<int, string> towerLevelUpEffectDescriptions = new Dictionary<int, string>()
    {
        { 1 , "발사체 개수" },
        { 2 , "사거리" },
        { 3 , "두께" },
        { 4 , "지속시간" },
        { 5 , "쿨타임" },
        { 6 , "연사속도" },
        { 7 , "필렛 개수" },
        { 8 , "파편 사거리" },
        { 9 , "파편 개수" },
        { 10 , "폭발 범위" },
        { 11 , "유도 횟수" },
        { 12 , "이동속도 감소량" },
        { 13 , "총알 속도 감소량" },
        { 14 , "정지 시간" },
        { 15 , "퍼지는 각도" },
        { 16 , "탄환 속도" },
    };

    public static string GetLevelUpEffectDescription(int var)
    {
        if (towerLevelUpEffectDescriptions.TryGetValue(var, out string description))
        {
            return description;
        }
        return string.Empty;
    }
}
