using Cysharp.Threading.Tasks;
using System.Threading;

public abstract class Consumable
{
    protected TowerManager towerManager;
    protected BasePlanet planet;
    protected ConsumalbeTable.Data consumData;

    private CancellationTokenSource ctr;

    public void Init(TowerManager towerManager , BasePlanet planet , ConsumalbeTable.Data data)
    {
        this.towerManager = towerManager;
        this.planet = planet;
        consumData = data;

        ctr = new CancellationTokenSource();
    }

    public void UseItem(float duration)
    {
        UseItemAsync(duration , ctr).Forget();
    }

    public void Release()
    {
        ctr.Cancel();
        ctr.Dispose();
    }

    protected abstract UniTaskVoid UseItemAsync(float duration , CancellationTokenSource ctr);
    protected abstract void ResetItem();
}