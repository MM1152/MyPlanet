using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class Terraforming : MonoBehaviour
{
    private WindowManager windowManager;

    [SerializeField] private TerraformingWindow terraformingWindow;

    [SerializeField] private StatusWindow statusWindow;
    private static BasePlanet basePlanet;
    private static Move defenceTowerMove;
    private static TowerManager towerManager;

    public Dictionary<int, TerraformingTable.Data> terraformingChoiceData = new Dictionary<int, TerraformingTable.Data>();    

    private Dictionary<TerraformingData.T_Effect_type, Action<float>> terraformingActions = new Dictionary<TerraformingData.T_Effect_type, Action<float>>()
    {
        { TerraformingData.T_Effect_type.IncreaseGoldGain, GoldGainUpgrade },
        { TerraformingData.T_Effect_type.IncreaseExpGain, ExpGainUpgrade },
        { TerraformingData.T_Effect_type.IncreaseMaxHealth, MaxHealthUpgrade },
        { TerraformingData.T_Effect_type.IncreaseAttackSpeed, AttackSpeedUpgrade },
        { TerraformingData.T_Effect_type.IncreaseAttackDamage, AttackDamageUpgrade },
        { TerraformingData.T_Effect_type.HealthRegeneration, HealthRegenerationUpgrade },
        { TerraformingData.T_Effect_type.IncreaseMovementSpeed, MovementSpeedUpgrade },
        { TerraformingData.T_Effect_type.IncreaseDefense, DefenseUpgrade },
    };

    private void Awake()
    {
        windowManager = GameObject.FindGameObjectWithTag(TagIds.WindowManagerTag)?.GetComponent<WindowManager>();
        basePlanet = GameObject.FindGameObjectWithTag(TagIds.PlayerTag)?.GetComponent<BasePlanet>();
        defenceTowerMove = GameObject.FindGameObjectWithTag(TagIds.DefenseTowerTag)?.GetComponent<Move>();
        towerManager = GameObject.FindGameObjectWithTag(TagIds.TowerManagerTag)?.GetComponent<TowerManager>();
    }

    public void SetPoint(int point)
    {
        if (Variable.IsTutorialActive) return;
        
        var data = DataTableManager.TerraformingTable.GetDataByPoint(point);

        if (data.Count != 2)
        {
            Debug.LogError($"Terraforming data count is not 2 for point: {point}");
            return;
        }
        var left = data[0];
        var right = data[1];

        terraformingWindow.SetUI(
            TerraformingData.GetTerraformingNameDataKey(left.Terra_name),
            TerraformingData.GetTerraformingDescriptionDataKey(left.T_description),
            TerraformingData.GetTerraformingNameDataKey(right.Terra_name),
            TerraformingData.GetTerraformingDescriptionDataKey(right.T_description),
            point,left.T_image,right.T_image );


        terraformingWindow.leftButton.onClick.RemoveAllListeners();
        terraformingWindow.leftButton.interactable = true;
        terraformingWindow.leftButton.onClick.AddListener(() => { ExecuteTerraforming((TerraformingData.T_Effect_type)left.T_Effect_type, left.T_effect_value); SetTerraformingState(point, left); windowManager.Close(); });

        terraformingWindow.rightButton.onClick.RemoveAllListeners();
        terraformingWindow.rightButton.interactable = true;
        terraformingWindow.rightButton.onClick.AddListener(() => { ExecuteTerraforming((TerraformingData.T_Effect_type)right.T_Effect_type, right.T_effect_value); SetTerraformingState(point, right); windowManager.Close(); });
        windowManager.Open(WindowIds.TerraformingWindow);
    }

   private void SetTerraformingState(int point ,TerraformingTable.Data data)
   {
         if (point > TerraformingData.terrformingOpenValues.Length)
              return;
    
         statusWindow.SetTerraformingState(point-1, data);
   }     

#if DEBUG_MODE
    private void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            Debug.Log("최대Hp증가");
            MaxHealthUpgrade(1f);
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            Debug.Log("공격속도 증가"); 
            AttackSpeedUpgrade(1f);
        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            Debug.Log("공격력 증가");   
            AttackDamageUpgrade(1f);
        }

        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            Debug.Log("체력재생 증가");
            HealthRegenerationUpgrade(0.01f);
        }

        if (Keyboard.current.digit5Key.wasPressedThisFrame)
        {
            Debug.Log("이동속도 증가"); 
            MovementSpeedUpgrade(1f);
        }

        if (Keyboard.current.digit6Key.wasPressedThisFrame)
        {
            Debug.Log("방어력 증가");   
            DefenseUpgrade(1f);
        }
    }
#endif  

    private void ExecuteTerraforming(TerraformingData.T_Effect_type effectType, float effectValue)
    {
        if (terraformingActions.ContainsKey(effectType))
        {
            terraformingActions[effectType].Invoke(effectValue);
        }
        else
        {
            Debug.LogError($"No action defined for effect type: {effectType}");
        }
    }

    private static void GoldGainUpgrade(float effectValue)
    {
#if DEBUG_MODE
        Debug.Log("골드 획득량 증가");
#endif
    }

    private static void ExpGainUpgrade(float effectValue)
    {
#if DEBUG_MODE
        Debug.Log("경험치 획득량 증가");
#endif
    }

    private static void MaxHealthUpgrade(float effectValue)
    {
        basePlanet.IncreaseMaxHealth(effectValue);
    }

    private static void AttackSpeedUpgrade(float effectValue)
    {
        towerManager.UpgradeAllTowerAttackSpeed(effectValue);
    }

    private static void AttackDamageUpgrade(float effectValue)
    {
        towerManager.UpgradeAllTowerATK(effectValue);
    }

    private static void HealthRegenerationUpgrade(float effectValue)
    {
        basePlanet.HealthRegenerationUpgrade(effectValue);
    }

    private static void MovementSpeedUpgrade(float effectValue)
    {
        defenceTowerMove.MoveSpeedUpgrade(effectValue);
    }

    private static void DefenseUpgrade(float effectValue)
    {
        basePlanet.DefenseUpgrade(effectValue);
    }
}
