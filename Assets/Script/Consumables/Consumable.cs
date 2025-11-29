using Cysharp.Threading.Tasks;
using System.Threading;
using Unity.VisualScripting;

public abstract class Consumable
{
    protected TowerManager towerManager;
    protected BasePlanet planet;
    protected ConsumalbeTable.Data consumData;
    public ConsumalbeTable.Data ConsumData => consumData;


    private CancellationTokenSource ctr;
    protected float duration;

    public void Init(TowerManager towerManager , BasePlanet planet , ConsumalbeTable.Data data)
    {
        this.towerManager = towerManager;
        this.planet = planet;
        consumData = data;

        if(ctr != null && !ctr.IsCancellationRequested)
        {
            Release();
        }

        ctr = new CancellationTokenSource();
    }

    public void UseItem()
    {
        UseItemAsync(consumData.duration, ctr).Forget();
        this.duration = consumData.duration;
    }

    public void Release()
    {
        ctr.Cancel();
        ctr.Dispose();
        ctr = null;
    }

    public void Update(float deltaTIme)
    {
        duration -= deltaTIme;
    }

    public float GetDuration()
    {
        return duration;
    }

    protected abstract UniTaskVoid UseItemAsync(float duration , CancellationTokenSource ctr);
    protected abstract void ResetItem();
}