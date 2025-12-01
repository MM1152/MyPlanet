using System.Collections.Generic;

public class ConsumableFactory : BaseFactory<Consumable>
{
    private Dictionary<int, Consumable> consumableFactory = new Dictionary<int, Consumable>()
    {
        {7101 , new PowerModule()},
        {7802 , new TempoModule()},
        {7203 , new RPMBooster()},
        {7909 , new DefenseShieldGenerator()},
        {71010 , new RepairPatch()},
        {7304 , new FireOutputAmplifier() },
        {7405 , new IceOutputAmplifier()},
        {7506 , new SteelOutputAmplifier()},
        {7707 , new DarkOutputAmplifier()},
        {7608 , new LightOutputAmplifier()}
    };

    public override Consumable CreateInstance(int id)
    {
        return consumableFactory[id];
    }

    public List<int> GetAllKeys()
    {
        return new List<int>(consumableFactory.Keys);

    }
}