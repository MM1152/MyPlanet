using System.Collections.Generic;

public class ConsumableFactory : BaseFactory<Consumable>
{
    private Dictionary<int, Consumable> consumableFactory = new Dictionary<int, Consumable>()
    {
        {7101 , new PowerModule()},
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