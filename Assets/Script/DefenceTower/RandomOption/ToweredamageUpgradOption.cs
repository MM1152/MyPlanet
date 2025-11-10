using System.Collections.Generic;

public class ToweredamageUpgradOption : RandomOptionBase
{
    public override void ResetRandomOption()
    {
        foreach(var tower in towers)
        {
            tower.AddBonusDamage(-5);
        }
    }

    public override void SetRandomOption()
    {
        foreach(var tower in towers)
        {
            tower.AddBonusDamage(5);
        }
    }
}