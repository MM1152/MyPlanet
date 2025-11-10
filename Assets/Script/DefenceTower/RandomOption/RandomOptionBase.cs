using System.Collections.Generic;

public abstract class RandomOptionBase
{
    protected BasePlanet planet;
    protected List<Tower> towers;

    public void Init(BasePlanet planet , List<Tower> towers)
    {
        this.planet = planet;
        this.towers = towers;
    }

    public abstract void ResetRandomOption();
    public abstract void SetRandomOption();
}