using System.Runtime.CompilerServices;

public class JupiterPassive : IPassive
{
    private TowerManager towerManger;
    private BasePlanet basePlanet;
    
    private float healCooldown = 5f;
    private float healTimer = 0f;

    public void ApplyPassive(PassiveSystem passiveSystem)
    {
        passiveSystem.OnUpdate = JupiterPassiveUpdate;
    }

    public void Init(TowerManager towerManager, BasePlanet basePlanet)
    {
        this.towerManger = towerManager;
        this.basePlanet = basePlanet;
    }

    private void JupiterPassiveUpdate(float deltaTime)
    {
        float percent = (float)basePlanet.hp / basePlanet.maxHp;

        healTimer += deltaTime;
        if(percent < 0.25f && healCooldown <= healTimer)
        {
            basePlanet.RepairHp(150);
            healTimer = 0;
        }
    }
}